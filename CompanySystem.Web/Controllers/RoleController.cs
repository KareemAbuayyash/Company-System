using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CompanySystem.Business.Interfaces;
using CompanySystem.Data.Models;
using CompanySystem.Data.Context;
using CompanySystem.Web.ViewModels;
using CompanySystem.Business.DTOs;

namespace CompanySystem.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly CompanySystemDbContext _context;

        public RoleController(IRoleService roleService, CompanySystemDbContext context)
        {
            _roleService = roleService;
            _context = context;
        }

        private static RoleViewModel MapToViewModel(RoleDto dto)
        {
            return new RoleViewModel
            {
                RoleId = dto.RoleId,
                RoleName = dto.RoleName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate,
                UpdatedBy = dto.UpdatedBy,
                UpdatedDate = dto.UpdatedDate
            };
        }

        public IActionResult Test()
        {
            return Content("Role Controller is working!");
        }

        // GET: Role
        public async Task<IActionResult> Index(string searchTerm, string sortBy = "name")
        {
            var result = await _roleService.GetRolesForIndexAsync(searchTerm, sortBy);
            var viewModels = result.Roles.Select(MapToViewModel).ToList();
            
            ViewBag.SearchTerm = result.SearchTerm;
            ViewBag.SortBy = result.SortBy;
            ViewBag.TotalRoles = result.TotalCount;
            ViewBag.HasSearch = result.HasSearch;

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> SearchRoles(string searchTerm, string sortBy = "name")
        {
            var roles = await _roleService.GetRolesForSearchAsync(searchTerm, sortBy);
            
            return Json(new
            {
                success = true,
                data = roles.Select(r => new
                {
                    roleId = r.RoleId,
                    roleName = r.RoleName,
                    description = r.Description,
                    isActive = r.IsActive,
                    createdBy = r.CreatedBy,
                    createdDate = r.CreatedDate.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    updatedBy = r.UpdatedBy,
                    updatedDate = r.UpdatedDate?.ToLocalTime().ToString("dd/MM/yyyy HH:mm")
                }),
                count = roles.Count(),
                searchTerm = searchTerm,
                sortBy = sortBy
            });
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var roleDto = await _roleService.GetRoleDtoByIdAsync(id);
            if (roleDto == null)
            {
                return NotFound();
            }
            return View(MapToViewModel(roleDto));
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View(new CreateRoleViewModel());
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var createDto = new CreateRoleDto
            {
                RoleName = model.RoleName,
                Description = model.Description,
                IsActive = model.IsActive
            };

            var result = await _roleService.CreateRoleAsync(createDto);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Role created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("RoleName", "A role with this name already exists.");
            return View(model);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var viewModel = new EditRoleViewModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Description = role.Description,
                IsActive = role.IsActive
            };

            return View(viewModel);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRoleViewModel model)
        {
            if (id != model.RoleId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editDto = new EditRoleDto
            {
                RoleId = model.RoleId,
                RoleName = model.RoleName,
                Description = model.Description,
                IsActive = model.IsActive
            };

            var result = await _roleService.UpdateRoleAsync(model.RoleId, editDto);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Role updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("RoleName", "A role with this name already exists.");
            return View(model);
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var roleDto = await _roleService.GetRoleDtoByIdAsync(id);
            if (roleDto == null)
            {
                return NotFound();
            }
            return View(MapToViewModel(roleDto));
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _roleService.SoftDeleteRoleAsync(id, "System");
                if (result)
                {
                    TempData["SuccessMessage"] = "Role deleted successfully!";
                }
                else
                {
                    // Check if there are users assigned to this role
                    var usersWithRole = await _context.Users
                        .IgnoreQueryFilters()
                        .Where(u => u.RoleId == id && u.IsDeleted == false)
                        .CountAsync();

                    if (usersWithRole > 0)
                    {
                        var role = await _roleService.GetRoleByIdAsync(id);
                        var roleName = role?.RoleName ?? "Unknown";
                        TempData["ErrorMessage"] = $"Cannot delete role '{roleName}' because it has {usersWithRole} user(s) assigned to it. Please reassign or delete the users first.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete role. The role may not exist or cannot be deleted.";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the role. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
