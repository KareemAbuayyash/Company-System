using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CompanySystem.Business.Interfaces;
using CompanySystem.Data.Models;
using CompanySystem.Web.ViewModels;
using CompanySystem.Business.DTOs;

namespace CompanySystem.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IDepartmentService _departmentService;

        public UserController(IUserService userService, IRoleService roleService, IDepartmentService departmentService)
        {
            _userService = userService;
            _roleService = roleService;
            _departmentService = departmentService;
        }

        private static UserViewModel MapToViewModel(UserDto dto)
        {
            return new UserViewModel
            {
                UserId = dto.UserId,
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                IsActive = dto.IsActive,
                LastLoginDate = dto.LastLoginDate,
                RoleId = dto.RoleId,
                RoleName = dto.RoleName,
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName,
                FullName = dto.FullName,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate,
                UpdatedBy = dto.UpdatedBy,
                UpdatedDate = dto.UpdatedDate
            };
        }

        public IActionResult Test()
        {
            return Content("User Controller is working!");
        }

        // GET: User
        public async Task<IActionResult> Index(string searchTerm, string sortBy = "name")
        {
            var result = await _userService.GetUsersForIndexAsync(searchTerm, sortBy);
            var viewModels = result.Users.Select(MapToViewModel).ToList();
            
            ViewBag.SearchTerm = result.SearchTerm;
            ViewBag.SortBy = result.SortBy;
            ViewBag.TotalUsers = result.TotalCount;
            ViewBag.HasSearch = result.HasSearch;

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string searchTerm, string sortBy = "name")
        {
            var users = await _userService.GetUsersForSearchAsync(searchTerm, sortBy);
            
            return Json(new
            {
                success = true,
                data = users.Select(u => new
                {
                    userId = u.UserId,
                    username = u.Username,
                    email = u.Email,
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    fullName = u.FullName,
                    phoneNumber = u.PhoneNumber,
                    isActive = u.IsActive,
                    lastLoginDate = u.LastLoginDate.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    roleName = u.RoleName,
                    departmentName = u.DepartmentName,
                    createdBy = u.CreatedBy,
                    createdDate = u.CreatedDate.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    updatedBy = u.UpdatedBy,
                    updatedDate = u.UpdatedDate?.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                }),
                count = users.Count(),
                searchTerm = searchTerm,
                sortBy = sortBy
            });
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userDto = await _userService.GetUserDtoByIdAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            return View(MapToViewModel(userDto));
        }

        // GET: User/Create
        public async Task<IActionResult> Create()
        {
            var roles = await _roleService.GetActiveRolesAsync();
            var departments = await _departmentService.GetAllDepartmentsAsync();

            ViewBag.Roles = roles.Select(r => new { r.RoleId, r.RoleName }).ToList();
            ViewBag.Departments = departments.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();

            return View(new CreateUserViewModel());
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var activeRoles = await _roleService.GetActiveRolesAsync();
                var allDepartments = await _departmentService.GetAllDepartmentsAsync();

                ViewBag.Roles = activeRoles.Select(r => new { r.RoleId, r.RoleName }).ToList();
                ViewBag.Departments = allDepartments.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();

                return View(model);
            }

            var createDto = new CreateUserDto
            {
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                IsActive = model.IsActive,
                RoleId = model.RoleId,
                DepartmentId = model.DepartmentId
            };

            var result = await _userService.CreateUserAsync(createDto);
            if (result != null)
            {
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("Username", "A user with this username or email already exists.");
            
            var activeRoles2 = await _roleService.GetActiveRolesAsync();
            var allDepartments2 = await _departmentService.GetAllDepartmentsAsync();

            ViewBag.Roles = activeRoles2.Select(r => new { r.RoleId, r.RoleName }).ToList();
            ViewBag.Departments = allDepartments2.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();

            return View(model);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleService.GetActiveRolesAsync();
            var departments = await _departmentService.GetAllDepartmentsAsync();

            ViewBag.Roles = roles.Select(r => new { r.RoleId, r.RoleName }).ToList();
            ViewBag.Departments = departments.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();

            var viewModel = new EditUserViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                DepartmentId = user.DepartmentId
            };

            return View(viewModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditUserViewModel model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var activeRoles = await _roleService.GetActiveRolesAsync();
                var allDepartments = await _departmentService.GetAllDepartmentsAsync();

                ViewBag.Roles = activeRoles.Select(r => new { r.RoleId, r.RoleName }).ToList();
                ViewBag.Departments = allDepartments.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();

                return View(model);
            }

            var editDto = new EditUserDto
            {
                UserId = model.UserId,
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                IsActive = model.IsActive,
                RoleId = model.RoleId,
                DepartmentId = model.DepartmentId
            };

            var result = await _userService.UpdateUserAsync(model.UserId, editDto);
            if (result != null)
            {
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("Username", "A user with this username or email already exists.");
            
            var roles = await _roleService.GetActiveRolesAsync();
            var departments = await _departmentService.GetAllDepartmentsAsync();

            ViewBag.Roles = roles.Select(r => new { r.RoleId, r.RoleName }).ToList();
            ViewBag.Departments = departments.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();

            return View(model);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userDto = await _userService.GetUserDtoByIdAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            return View(MapToViewModel(userDto));
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _userService.SoftDeleteUserAsync(id, "System");
                if (result)
                {
                    TempData["SuccessMessage"] = "User deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete user. The user may not exist or cannot be deleted.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
