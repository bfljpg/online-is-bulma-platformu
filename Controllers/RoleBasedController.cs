
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
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı.";
                return View();
            }

            // Kullanıcı bilgilerini Session'a kaydet
            // Oturum değişkenlerini ayarla
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("UserRole", user.Role);

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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string firstName, string lastName, string email, string password, string role)
        {
            // Kullanıcı mevcut mu kontrol et
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);

            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Bu e-posta adresi zaten kayıtlı.";
                return View();
            }

            // Yeni kullanıcı oluştur
            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = password,
                Role = role
            };

            // Veritabanına ekle
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Otomatik giriş yapılabilir veya giriş sayfasına yönlendirebilir
            ViewBag.SuccessMessage = "Kayıt başarılı! Lütfen giriş yapın.";
            return RedirectToAction("Login");
        }
    }
}
