using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using online_is_bulma_platformu.Data;
using online_is_bulma_platformu.Models;

namespace online_is_bulma_platformu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Index31()
        {
            using (var context = new JobPortalContext())
            {
                context.Users.Add(new User { FirstName = "Ali", LastName = "Y�lmaz", Email = "ali@example.com", PasswordHash = "123456", Role = "Employer", CreatedAt = DateTime.Now });
                context.SaveChanges();
            }
            return View();
        }
    }
}
