using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Data.Models;
using CompanySystem.Web.ViewModels;

namespace CompanySystem.Web.Controllers
{
    [Authorize(Policy = "AdministratorOnly")]
    public class RoleController : Controller
    {
        private readonly IAuthService _authService;

        public RoleController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _authService.GetAllActiveRolesAsync();
            return View(roles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var role = await _authService.GetActiveRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // For now, we'll create a simple role
                // In a real application, you might want to add a RoleService
                TempData["SuccessMessage"] = "Role creation functionality will be implemented with a dedicated RoleService.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _authService.GetActiveRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.RoleName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRoleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // For now, we'll show a message
                // In a real application, you might want to add a RoleService
                TempData["SuccessMessage"] = "Role update functionality will be implemented with a dedicated RoleService.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = await _authService.GetActiveRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // For now, we'll show a message
            // In a real application, you might want to add a RoleService
            TempData["SuccessMessage"] = "Role deletion functionality will be implemented with a dedicated RoleService.";
            return RedirectToAction(nameof(Index));
        }
    }
}
