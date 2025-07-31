using Microsoft.AspNetCore.Mvc;
using CompanySystem.Business.Interfaces;
using CompanySystem.Data.Models;
using CompanySystem.Web.ViewModels;

namespace CompanySystem.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: Department
        public async Task<IActionResult> Index(string searchTerm, string sortBy = "name")
        {
            // Use the efficient filtered method that handles everything at database level
            var departments = await _departmentService.GetFilteredDepartmentsAsync(searchTerm, sortBy);

            var viewModels = departments.Select(d => new DepartmentViewModel
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                CreatedBy = d.CreatedBy,
                CreatedDate = d.CreatedDate,
                UpdatedBy = d.UpdatedBy,
                UpdatedDate = d.UpdatedDate
            }).ToList();

            // Pass search and sort parameters to view
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;
            ViewBag.TotalDepartments = viewModels.Count;
            ViewBag.HasSearch = !string.IsNullOrWhiteSpace(searchTerm);

            return View(viewModels);
        }

        // AJAX endpoint for fast search
        [HttpGet]
        public async Task<IActionResult> SearchDepartments(string searchTerm, string sortBy = "name")
        {
            var departments = await _departmentService.GetFilteredDepartmentsAsync(searchTerm, sortBy);
            
            var result = departments.Select(d => new
            {
                departmentId = d.DepartmentId,
                departmentName = d.DepartmentName,
                createdBy = d.CreatedBy,
                createdDate = d.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                updatedBy = d.UpdatedBy,
                updatedDate = d.UpdatedDate?.ToString("dd/MM/yyyy HH:mm")
            }).ToList();

            return Json(new
            {
                success = true,
                data = result,
                count = result.Count,
                searchTerm = searchTerm,
                sortBy = sortBy
            });
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewModel = new DepartmentViewModel
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                CreatedBy = department.CreatedBy,
                CreatedDate = department.CreatedDate,
                UpdatedBy = department.UpdatedBy,
                UpdatedDate = department.UpdatedDate
            };

            return View(viewModel);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            return View(new CreateDepartmentViewModel());
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if department name already exists
                if (await _departmentService.DepartmentNameExistsAsync(model.DepartmentName))
                {
                    ModelState.AddModelError("DepartmentName", "A department with this name already exists.");
                    return View(model);
                }

                var department = new Department
                {
                    DepartmentName = model.DepartmentName,
                    CreatedBy = "System", // TODO: Replace with actual user when authentication is implemented
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _departmentService.CreateDepartmentAsync(department);
                TempData["SuccessMessage"] = "Department created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewModel = new EditDepartmentViewModel
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName
            };

            return View(viewModel);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDepartmentViewModel model)
        {
            if (id != model.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if department name already exists (excluding current department)
                if (await _departmentService.DepartmentNameExistsAsync(model.DepartmentName, id))
                {
                    ModelState.AddModelError("DepartmentName", "A department with this name already exists.");
                    return View(model);
                }

                var department = new Department
                {
                    DepartmentId = model.DepartmentId,
                    DepartmentName = model.DepartmentName,
                    UpdatedBy = "System" // TODO: Replace with actual user when authentication is implemented
                };

                try
                {
                    await _departmentService.UpdateDepartmentAsync(department);
                    TempData["SuccessMessage"] = "Department updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException)
                {
                    return NotFound();
                }
            }

            return View(model);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var viewModel = new DepartmentViewModel
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                CreatedBy = department.CreatedBy,
                CreatedDate = department.CreatedDate,
                UpdatedBy = department.UpdatedBy,
                UpdatedDate = department.UpdatedDate
            };

            return View(viewModel);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _departmentService.SoftDeleteDepartmentAsync(id, "System"); // TODO: Replace with actual user
            if (result)
            {
                TempData["SuccessMessage"] = "Department deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete department.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
