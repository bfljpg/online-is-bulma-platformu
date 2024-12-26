
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_is_bulma_platformu.Data;
using online_is_bulma_platformu.Models;
using System.Linq;

namespace online_is_bulma_platformu.Controllers
{
    public class RoleBasedController : Controller
    {
        private readonly JobPortalContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleBasedController(JobPortalContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // User tablosundan kullanıcının e-posta ve şifresini kontrol et
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı.";
                return View();
            }

            // Kullanıcı bilgilerini Session'a kaydet
            _httpContextAccessor.HttpContext.Session.SetString("UserRole", user.Role);
            _httpContextAccessor.HttpContext.Session.SetString("UserName", user.FirstName);

            // Kullanıcının rolüne göre yönlendirme
            if (user.Role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (user.Role == "Employer")
            {
                return RedirectToAction("Dashboard", "Employer");
            }
            else if (user.Role == "JobSeeker")
            {
                return RedirectToAction("Dashboard", "JobSeeker");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login", "RoleBased");
        }

        [HttpGet]
        public IActionResult TestConnection()
        {
            try
            {
                var users = _context.Users.ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
