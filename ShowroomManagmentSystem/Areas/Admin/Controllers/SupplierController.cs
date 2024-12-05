namespace ShowroomManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext ctx;
        private readonly INotyfService _toastNotification;

        public SupplierController(ApplicationDbContext context, INotyfService notyfService)
        {
            ctx = context;
            _toastNotification = notyfService;
        }

        public async Task<IActionResult> Index(string? name, int? page = 1)
        {
            var pageNo = page ?? 1;
            var pageSize = 5;
            ViewBag.names = name;
            var sup = await ctx.Suppliers.OrderByDescending(x => x.SupplierId).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                sup = await ctx.Suppliers.Where(x => x.SupplierName.Contains(name)).ToListAsync();
            }
            var pageData = sup.ToPagedList(pageNo, pageSize);
            return View(pageData);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSupplier(Supplier data)
        {
            ctx.Suppliers.Add(data);
            ctx.SaveChanges();
            _toastNotification.Success("Thêm mới thành công !", 3);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            Supplier obj = await ctx.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == Id);
            if (obj != null)
            {
                ctx.Suppliers.Remove(obj);
                await ctx.SaveChangesAsync();
            }
            _toastNotification.Success("Xóa thành công !", 3);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int Id)
        {
            Supplier obj = await ctx.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == Id);
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Supplier data)
        {
            ctx.Suppliers.Update(data);
            await ctx.SaveChangesAsync();
            _toastNotification.Success("Cập nhật thành công !", 3);
            return RedirectToAction("Index");
        }
    }
}
