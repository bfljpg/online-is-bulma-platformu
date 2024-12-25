using Microsoft.AspNetCore.Mvc;
using online_is_bulma_platformu.Data;
using online_is_bulma_platformu.Models;
using System.Linq;

namespace online_is_bulma_platformu.Controllers
{
    public class EmployerController : Controller
    {
        private readonly JobPortalContext _context;

        public EmployerController(JobPortalContext context)
        {
            _context = context;
        }

        public IActionResult MyJobs(int employerId)
        {
            var myJobs = _context.JobListings.Where(j => j.EmployerId == employerId).ToList();
            return View(myJobs);
        }

        public IActionResult CreateJob()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateJob(JobListing job)
        {
            _context.JobListings.Add(job);
            _context.SaveChanges();
            return RedirectToAction("MyJobs", new { employerId = job.EmployerId });
        }

        public IActionResult DeleteJob(int id)
        {
            var job = _context.JobListings.Find(id);
            if (job != null)
            {
                _context.JobListings.Remove(job);
                _context.SaveChanges();
            }
            return RedirectToAction("MyJobs", new { employerId = job.EmployerId });
        }
    }
}
