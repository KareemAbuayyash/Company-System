using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Business.DTOs.Auth;
using CompanySystem.Data.Models;
using CompanySystem.Web.ViewModels;
using System.Security.Claims;

namespace CompanySystem.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize(Policy = "ManagementLevel")]
        public async Task<IActionResult> Index()
        {
            var users = await _authService.GetAllActiveUsersAsync();
            return View(users);
        }

        [Authorize(Policy = "ManagementLevel")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _authService.GetActiveUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> Create()
        {
            var roles = await _authService.GetAllActiveRolesAsync();
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerModel = new RegisterModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    SerialNumber = model.SerialNumber,
                    PhoneNumber = model.PhoneNumber,
                    RoleId = model.RoleId,
                    DepartmentId = model.DepartmentId
                };

                var result = await _authService.RegisterAsync(registerModel, User.Identity?.Name);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "User created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }

            var roles = await _authService.GetAllActiveRolesAsync();
            ViewBag.Roles = roles;
            return View(model);
        }

        [Authorize(Policy = "ManagementLevel")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _authService.GetActiveUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _authService.GetAllActiveRolesAsync();
            ViewBag.Roles = roles;

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                SerialNumber = user.SerialNumber,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                DepartmentId = user.DepartmentId,
                IsActive = user.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "ManagementLevel")]
        public async Task<IActionResult> Edit(int id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _authService.GetActiveUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.SerialNumber = model.SerialNumber;
                user.PhoneNumber = model.PhoneNumber;
                user.RoleId = model.RoleId;
                user.DepartmentId = model.DepartmentId;
                user.IsActive = model.IsActive;

                var success = await _authService.UpdateUserAsync(user, User.Identity?.Name);

                if (success)
                {
                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update user.");
                }
            }

            var roles = await _authService.GetAllActiveRolesAsync();
            ViewBag.Roles = roles;
            return View(model);
        }

        [Authorize(Policy = "ManagementLevel")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _authService.GetActiveUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "ManagementLevel")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _authService.SoftDeleteUserAsync(id, User.Identity?.Name);

            if (success)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete user.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _authService.GetActiveUserByIdAsync(userId);
            
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var success = await _authService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword, User.Identity?.Name);

                if (success)
                {
                    TempData["SuccessMessage"] = "Password changed successfully!";
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to change password. Please check your current password.");
                }
            }

            return View(model);
        }
    }
}
