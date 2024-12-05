namespace ShowroomManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public SupplierController(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _toastNotification = notyfService;
        }

        public async Task<IActionResult> Index(string? name, string? sort, int? page = 1)
        {
            var pageNo = page ?? 1;
            var pageSize = 8;
            ViewBag.names = name;
            ViewBag.sorts = sort;
            // Filtering by name if provided
            var suppliersQuery = _context.Suppliers.OrderByDescending(x => x.SupplierId).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                suppliersQuery = suppliersQuery.Where(x => x.SupplierName.Contains(name));
                ViewBag.names = name;
            }

            // Fetching the data with paging
            var suppliers = suppliersQuery.ToPagedList(pageNo, pageSize);
            ViewData["CurrentPage"] = pageNo;  // Store the current page in ViewData to use in the delete link.
            return View(suppliers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(Supplier data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Suppliers.AddAsync(data);
                    await _context.SaveChangesAsync();
                    _toastNotification.Success("Thêm mới thành công !", 3);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _toastNotification.Error("Đã có lỗi xảy ra: " + ex.Message, 5);
                    return RedirectToAction(nameof(Index));
                }
            }

            // If the model is invalid, show an error notification and re-render the form
            _toastNotification.Error("Vui lòng kiểm tra lại thông tin.", 5);
            return View(data); // You can return the same view to show validation messages
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == Id);
            // If supplier is not found, return 404 or redirect to the Index page
            if (supplier == null)
            {
                _toastNotification.Error("Nhà cung cấp không tồn tại.", 5);
                return RedirectToAction("Index");
            }
            // Return the supplier to the Edit view
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    _toastNotification.Success("Cập nhật danh mục thành công !", 3);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    _toastNotification.Error("Có lỗi xảy ra khi cập nhật !", 3);
                    return RedirectToAction(nameof(Edit));
                }
            }
            return RedirectToAction(nameof(Edit));
        }

        public async Task<IActionResult> Delete(int id, int? page)
        {
            try
            {
                // Find the supplier to delete
                Supplier obj = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);

                if (obj == null)
                {
                    // If the supplier doesn't exist, show an error message
                    _toastNotification.Error("Nhà cung cấp không tồn tại.", 5);
                    return RedirectToAction("Index");
                }

                // Remove the supplier from the database
                _context.Suppliers.Remove(obj);
                await _context.SaveChangesAsync();

                // Show a success notification
                _toastNotification.Success("Xóa thành công !", 3);
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the deletion
                _toastNotification.Error("Đã có lỗi xảy ra khi xóa: " + ex.Message, 5);
            }

            // Redirect to the Index page with the same page number
            return RedirectToAction("Index", new { page = page ?? 1 });
        }
    }
}
