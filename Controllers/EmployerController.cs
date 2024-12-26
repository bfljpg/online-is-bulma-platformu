using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_is_bulma_platformu.Data;
using online_is_bulma_platformu.Models;

namespace online_is_bulma_platformu.Controllers
{
    public class EmployerController : Controller
    {
        private readonly JobPortalContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployerController(IHttpContextAccessor httpContextAccessor, JobPortalContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    ViewData["UserName"] = user.FirstName + " " + user.LastName;
                }
                else
                {
                    ViewBag.UserName = "Bilinmeyen Kullanýcý";
                }
            }


            if (_httpContextAccessor.HttpContext.Session.GetString("UserRole") != "Employer")
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


        [HttpGet]
        public IActionResult JobListings()
        {
            var employerId = HttpContext.Session.GetInt32("UserId");
            if (employerId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            var listings = _context.JobListings
                .Where(j => j.EmployerId == employerId)
                .ToList();

            return View(listings);
        }



        // Yeni ilan ekleme sayfasý
        [HttpGet]
        public IActionResult CreateJobListing()
        {
            return View();
        }

        // Yeni ilan ekle
        [HttpPost]
        public IActionResult CreateJobListing(JobListing jobListing)
        {
            var employerId = HttpContext.Session.GetInt32("UserId");
            if (employerId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            jobListing.EmployerId = employerId.Value;
            jobListing.CreatedAt = DateTime.Now;

            _context.JobListings.Add(jobListing);
            _context.SaveChanges();

            return RedirectToAction("JobListings");
        }

        // Ýlan sil
        [HttpPost]
        public IActionResult DeleteJobListing(int id)
        {
            var jobListing = _context.JobListings.FirstOrDefault(j => j.Id == id);
            if (jobListing == null || jobListing.EmployerId != HttpContext.Session.GetInt32("UserId"))
            {
                return Unauthorized();
            }

            _context.JobListings.Remove(jobListing);
            _context.SaveChanges();

            return RedirectToAction("JobListings");
        }

        [HttpGet]
        public IActionResult Applications(int jobListingId)
        {
            var employerId = HttpContext.Session.GetInt32("UserId");
            if (employerId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            var applications = _context.JobApplications
                .Where(a => a.JobListingId == jobListingId && a.JobListing.EmployerId == employerId)
                .Select(a => new
                {
                    a.Id,
                    JobSeekerName = a.JobSeeker.FirstName + " " + a.JobSeeker.LastName,
                    a.Message,
                    a.ApplicationDate
                }).ToList();

            return View(applications);
        }


        [HttpGet]
        public IActionResult TestSession()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var message = userId == null ? "User not logged in." : $"Logged in user ID: {userId}";
            return Ok(message);
        }
    }
}
