using Microsoft.AspNetCore.Mvc;
using online_is_bulma_platformu.Data;
using System.Linq;

namespace online_is_bulma_platformu.Controllers
{
    public class AdminController : Controller
    {
        private readonly JobPortalContext _context;

        public AdminController(JobPortalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var allUsers = _context.Users.ToList();
            return View(allUsers);
        }

        public IActionResult ManageJobs()
        {
            var allJobs = _context.JobListings.ToList();
            return View(allJobs);
        }

        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
