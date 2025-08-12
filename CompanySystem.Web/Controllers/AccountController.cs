using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanySystem.Business.Interfaces.Auth;
using CompanySystem.Web.ViewModels;
using System.Security.Claims;
using CompanySystem.Data.Data;
using CompanySystem.Business.Services.Auth;

namespace CompanySystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly CompanyDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthService authService, 
            CompanyDbContext context, 
            IPasswordHasher passwordHasher,
            ILogger<AccountController> logger)
        {
            _authService = authService;
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            _logger.LogInformation("Login GET action called");
            
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            _logger.LogInformation("Login POST action called for email: {Email}", model?.Email ?? "null");
            
            ViewData["ReturnUrl"] = returnUrl;

            try
            {
                // No validation - just proceed with authentication
                var email = model?.Email ?? "";
                var password = model?.Password ?? "";

                _logger.LogInformation("Attempting login for: {Email}", email);

                // Ensure admin user is set up
                await EnsureAdminUserSetup();

                // Try authentication
                var result = await _authService.LoginAsync(email, password);

                if (result.Success && result.User != null)
                {
                    _logger.LogInformation("Authentication successful for: {Email}", email);
                    
                    // Create claims for the user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString()),
                        new Claim(ClaimTypes.Name, result.User.FullName),
                        new Claim(ClaimTypes.Email, result.User.Email),
                        new Claim("Role", result.User.Role?.RoleName ?? "Employee"),
                        new Claim("SerialNumber", result.User.SerialNumber)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "CompanySystem");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model?.RememberMe ?? false,
                        ExpiresUtc = result.TokenExpiry ?? DateTimeOffset.UtcNow.AddHours(24)
                    };

                    await HttpContext.SignInAsync("CompanySystem", new ClaimsPrincipal(claimsIdentity), authProperties);

                    _logger.LogInformation("User {Email} signed in successfully", email);

                    // Redirect to return URL or home page
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Authentication failed for: {Email}. Reason: {Reason}", 
                        email, result.Message);
                    
                    // Just show error message, no validation
                    TempData["ErrorMessage"] = result.Message ?? "Login failed. Please check your credentials.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", model?.Email ?? "null");
                TempData["ErrorMessage"] = "An error occurred during login. Please try again.";
            }

            return View(model ?? new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("CompanySystem");
                _logger.LogInformation("User logged out successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task EnsureAdminUserSetup()
        {
            try
            {
                _logger.LogInformation("Checking admin user setup");
                
                var adminUser = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == "admin@company.com");

                if (adminUser == null)
                {
                    _logger.LogWarning("Admin user not found in database");
                    return;
                }

                if (adminUser.PasswordHash == "TEMP_HASH")
                {
                    _logger.LogInformation("Updating admin password from TEMP_HASH");
                    adminUser.PasswordHash = _passwordHasher.HashPassword("Admin123!");
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Admin password updated successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ensuring admin user setup");
            }
        }

        #if DEBUG
        [HttpGet]
        public async Task<IActionResult> ResetAdminPassword()
        {
            try
            {
                var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@company.com");
                if (adminUser != null)
                {
                    adminUser.PasswordHash = _passwordHasher.HashPassword("Admin123!");
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Admin password reset successfully");
                    return Json(new { success = true, message = "Admin password reset successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Admin user not found!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting admin password");
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DebugAdmin()
        {
            try
            {
                var adminUser = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.Department)
                    .FirstOrDefaultAsync(u => u.Email == "admin@company.com");
                
                if (adminUser != null)
                {
                    var isPasswordValid = _passwordHasher.VerifyPassword("Admin123!", adminUser.PasswordHash);
                    var debugInfo = new
                    {
                        Found = true,
                        Id = adminUser.Id,
                        Email = adminUser.Email,
                        IsActive = adminUser.IsActive,
                        IsDeleted = adminUser.IsDeleted,
                        Role = adminUser.Role?.RoleName ?? "No Role",
                        Department = adminUser.Department?.DepartmentName ?? "No Department",
                        PasswordValid = isPasswordValid
                    };
                    
                    return Json(debugInfo);
                }
                else
                {
                    return Json(new { Found = false, Message = "Admin user not found!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error debugging admin user");
                return Json(new { Found = false, Error = ex.Message });
            }
        }
        #endif
    }
}