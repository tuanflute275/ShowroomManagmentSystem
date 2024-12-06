using Microsoft.AspNetCore.Mvc;

namespace ShowroomManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public AccountController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            ctx = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index(string? name, string? ascName, string? descName, int? page = 1)
        {
            var pageSize = page ?? 1;
            var limit = 5;
            ViewBag.names = name;
            var acc = await ctx.Accounts.OrderByDescending(x => x.AccountId).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                acc = await ctx.Accounts.Where(x => x.Username.Contains(name)).ToListAsync();
            }
            if (name == "ascName")
            {
                acc = await ctx.Accounts.OrderBy(x => x.Username).ToListAsync();
            }
            else if (name == "descName")
            {
                acc = await ctx.Accounts.OrderByDescending(x => x.Username).ToListAsync();
            }
            var pageData = acc.ToPagedList(pageSize, limit);
            return View(pageData);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(IFormFile fileUpload, Account data)
        {
            if (fileUpload != null)
            {
                var rootPath = _environment.ContentRootPath;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("UserAvatar", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", fileUpload.FileName);

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }
                data.Avatar = fileUpload.FileName;
            }
            await ctx.Accounts.AddAsync(data);
            await ctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Account account = ctx.Accounts.FirstOrDefault(a => a.AccountId == id);
            if (account != null)
            {
                ctx.Accounts.Remove(account);
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public IActionResult Update(int id)
        {
            Account obj = ctx.Accounts.FirstOrDefault(x => x.AccountId == id);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, string? oldImage, IFormFile fileUpload, Account data)
        {
            try
            {
                if (id != data.AccountId)
                {
                    return NotFound();
                }
                if (fileUpload != null)
                {
                    var rootPath = _environment.ContentRootPath;
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("UserAvatar", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                    }

                    var path = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", fileUpload.FileName);

                    if (!string.IsNullOrEmpty(oldImage))
                    {
                        var pathOldFile = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", oldImage);
                        System.IO.File.Delete(pathOldFile);

                    }

                    using (var file = System.IO.File.Create(path))
                    {
                        fileUpload.CopyTo(file);
                    }

                    data.Avatar = fileUpload.FileName;
                }
                else
                {
                    data.Avatar = oldImage;
                }
                ctx.Accounts.Update(data);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                return View(ex);
            }
        }
    }
}
