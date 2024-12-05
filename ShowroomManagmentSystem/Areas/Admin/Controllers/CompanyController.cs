using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace ShowroomManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext ctx;

        public CompanyController(ApplicationDbContext context)
        {
            ctx = context;
        }
        public async Task<IActionResult> Index(string? name, int? page = 1)
        {
            var pageSize = page ?? 1;
            var limit = 5;
            ViewBag.names = name;
            var com = await ctx.Companies.OrderByDescending(x => x.CompanyId).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                com = await ctx.Companies.Where(x => x.CompanyName.Contains(name)).ToListAsync();
            }
            var pageData = com.ToPagedList(pageSize, limit);
            return View(pageData);
        }
    }
}
