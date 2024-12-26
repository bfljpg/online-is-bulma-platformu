using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_is_bulma_platformu.Data;
using online_is_bulma_platformu.Models;

namespace online_is_bulma_platformu.Controllers
{
    public class JobSeekerController : Controller
    {
        private readonly JobPortalContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JobSeekerController(IHttpContextAccessor httpContextAccessor, JobPortalContext context)
        {
            _context = context;
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

        // T�m ilanlar� listele
        [HttpGet]
        public IActionResult JobListings()
        {
            var jobListings = _context.JobListings.ToList();
            return View(jobListings);
        }

        // Ba�vuru G�nderme
        [HttpPost]
        public IActionResult Apply(int jobListingId, string message)
        {
            var jobSeekerId = HttpContext.Session.GetInt32("UserId");
            if (jobSeekerId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            var jobApplication = new Models.JobApplication
            {
                JobListingId = jobListingId,
                JobSeekerId = jobSeekerId.Value,
                ApplicationDate = DateTime.Now,
                Message = message
            };

            _context.JobApplications.Add(jobApplication);
            _context.SaveChanges();

            return RedirectToAction("JobListings");
        }

        [HttpGet]
        public IActionResult Messages()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            // �� arayan�n ald��� mesajlar� �ek
            var messages = _context.UserMessages
                .Where(m => m.ReceiverId == userId)
                .Select(m => new
                {
                    m.SenderId,                             // G�nderen ID'si eklendi
                    SenderName = m.Sender.FirstName + " " + m.Sender.LastName,
                    JobTitle = m.JobListing.Title,
                    m.Message,
                    m.SentAt,
                    m.JobListingId                          // �lan ID'si eklendi
                })
                .OrderByDescending(m => m.SentAt)          // En son mesajlar� �stte g�stermek i�in s�ralama
                .ToList();

            return View(messages);
        }


        [HttpPost]
        public IActionResult ReplyMessage(int receiverId, int jobListingId, string message)
        {
            var senderId = HttpContext.Session.GetInt32("UserId");
            if (senderId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            // Yan�t olarak yeni mesaj olu�tur
            var replyMessage = new UserMessage
            {
                SenderId = senderId.Value, // Yan�t g�nderen i� arayan
                ReceiverId = receiverId,  // Yan�t alan i�veren
                JobListingId = jobListingId, // �lgili i� ilan�
                Message = message,
                SentAt = DateTime.Now
            };

            _context.UserMessages.Add(replyMessage);
            _context.SaveChanges();

            return RedirectToAction("Messages"); // Mesajlar sayfas�na geri d�n
        }


        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login", "RoleBased");
        }
    }
}
