using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return Content("Company System - Training Project is working!");
        }
    }
} 