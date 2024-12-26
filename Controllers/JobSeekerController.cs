using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace online_is_bulma_platformu.Controllers
{
    public class JobSeekerController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JobSeekerController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Dashboard()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("UserRole") != "JobSeeker")
            {
                return RedirectToAction("Login", "RoleBased");
            }

            ViewBag.UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login", "RoleBased");
        }
    }
}
