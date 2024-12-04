using Microsoft.AspNetCore.Authentication.Facebook;
using Newtonsoft.Json;

namespace ShowroomManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;
        private readonly IMapper _mapper;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public AuthController(INotyfService toastNotification, ApplicationDbContext context, IMapper mapper, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _toastNotification = toastNotification;
            _mapper = mapper;
            _environment = environment;
        }

        [HttpGet]
        [Route("signin-facebook")]
        public async Task<ActionResult> FacebookLogin()
        {
            var redirectUrl = Url.Action("FacebookCallback", "Auth");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("facebook-callback")]
        public async Task<ActionResult> FacebookCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                // Nếu xác thực thất bại, có thể xử lý theo nhu cầu của bạn, ví dụ như redirect tới trang lỗi
                return RedirectToAction("Login", "Auth");
            }

            // Lấy thông tin người dùng từ Facebook
            var accessToken = authenticateResult.Properties.GetTokenValue("access_token");

            // Có thể gọi API Facebook để lấy thêm thông tin người dùng nếu cần
            var userInfo = await GetFacebookUserInfo(accessToken);

            // Đăng nhập người dùng vào ứng dụng
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfo.Name),
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                // Thêm các claim khác nếu cần
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Redirect đến trang chủ hoặc một trang sau khi đăng nhập thành công
            return RedirectToAction("Index", "Home");
        }

        // Hàm để gọi API Facebook và lấy thông tin người dùng
        private async Task<dynamic> GetFacebookUserInfo(string accessToken)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"https://graph.facebook.com/me?access_token={accessToken}&fields=id,name,picture");
            return JsonConvert.DeserializeObject<dynamic>(response);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Index(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            try
            {
                // validate input data
                if (!ModelState.IsValid || model.Password.Length < 6)
                {
                    TempData["error"] = "Password must be at least 6 characters long!";
                    return View(model);
                }

                // bussiness login
                Account accountExist = await _context.Accounts.FirstOrDefaultAsync(x =>
                        x.Email == model.UsernameOrEmail || x.Username == model.UsernameOrEmail);
                if (accountExist == null || !BCrypt.Net.BCrypt.Verify(model.Password, accountExist.Password))
                {
                    TempData["error"] = "Invalid username or password!";
                    return View(model);
                }

                string roleClaim = accountExist.Role switch
                {
                    0 => "User",
                    1 => "Admin",
                    2 => "Employee",
                    _ => "User"
                };

                // Save data login to cookie
                var claims = new List<Claim>
            {
                new Claim("userId", accountExist.AccountId.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Name, accountExist.Username ?? string.Empty),
                new Claim("userFullName", accountExist.FullName ?? string.Empty),
                new Claim(ClaimTypes.Email, accountExist.Email ?? string.Empty),
                new Claim("avatar", accountExist.Avatar ?? "default.png"),
                new Claim("Role", roleClaim ?? "User")
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                _toastNotification.Success("Login successfully!");
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return accountExist.Role == 0 ? Redirect("/") : RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                // validate input data
                if (model.Password != null && model.Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "Password must be at least 6 characters long!");
                    TempData["error"] = "Password must be at least 6 characters long!";
                }
                if (AccountUserNameExists(model.UserName))
                {
                    ModelState.AddModelError("UserName", "The username already exists in the system!");
                }

                if (AccountEmailExists(model.Email))
                {
                    ModelState.AddModelError("UserEmail", "The email already exists in the system!");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, 12);
                Account account = new Account
                {
                    Username = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = passwordHash,
                    Role = 0
                };

                await _context.AddAsync(account);
                await _context.SaveChangesAsync();

                // Gửi email xác nhận đăng ký
                EmailUtil.SendEmailAsync(account.Email, "Account Registration", Util.BodyRegisterMail(account.FullName));
                TempData["message"] = "Account registration successful!";
                return Redirect("/login");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Route("forgot-password")]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (AccountEmailExists(forgotPasswordModel.Email))
                    {
                        var acc = _context.Accounts.FirstOrDefault(x => x.Email == forgotPasswordModel.Email);
                        var pass = Util.CreateRandomPassword(8);
                        acc.Password = pass;
                        _context.Update(acc);
                        await _context.SaveChangesAsync();
                        EmailUtil.SendEmailAsync(forgotPasswordModel.Email, "Forgot password", Util.BodyResetPasswordMail(pass));
                        TempData["message"] = "Forgot password successful, please check your email!";
                        return Redirect("/login");
                    }
                    else
                    {
                        TempData["error"] = "Account does not exist!";
                        return View(forgotPasswordModel);
                    }
                }
                return View(forgotPasswordModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View(forgotPasswordModel);
            }
        }
       
        public async Task<IActionResult> Logout()
        {
            _toastNotification.Success("Logout successfully!", 3);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/login");
        }
       
       

        private bool AccountEmailExists(string email)
        {
            return _context.Accounts.Any(e => e.Email == email);
        }
        private bool AccountUserNameExists(string username)
        {
            return _context.Accounts.Any(e => e.Username == username);
        }

        private bool AccountUserNameExists(string userName, int currentAccountId)
        {
            return _context.Accounts.Any(a => a.Username == userName && a.AccountId != currentAccountId);
        }

        private bool AccountEmailExists(string email, int currentAccountId)
        {
            return _context.Accounts.Any(a => a.Email == email && a.AccountId != currentAccountId);
        }
    }
}
