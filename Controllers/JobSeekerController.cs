using Microsoft.AspNetCore.Mvc;
using online_is_bulma_platformu.Data;
using online_is_bulma_platformu.Models;
using System.Linq;

namespace online_is_bulma_platformu.Controllers
{
    public class JobSeekerController : Controller
    {
        private readonly JobPortalContext _context;

        public JobSeekerController(JobPortalContext context)
        {
            _context = context;
        }

        public IActionResult JobListings()
        {
            var jobs = _context.JobListings.ToList();
            return View(jobs);
        }

        public IActionResult Apply(int jobId)
        {
            var job = _context.JobListings.Find(jobId);
            return View(job);
        }

        [HttpPost]
        public IActionResult Apply(JobApplication application)
        {
            _context.JobApplications.Add(application);
            _context.SaveChanges();
            return RedirectToAction("JobListings");
        }
    }
}
