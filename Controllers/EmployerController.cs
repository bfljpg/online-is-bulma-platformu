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
        public IActionResult Applications()
        {
            var employerId = HttpContext.Session.GetInt32("UserId");
            if (employerId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            // Ýþverene ait tüm baþvurularý getir
            var applications = _context.JobApplications
                .Where(a => a.JobListing.EmployerId == employerId)
                .Select(a => new
                {
                    a.Id,
                    JobSeekerName = a.JobSeeker.FirstName + " " + a.JobSeeker.LastName,
                    a.Message,
                    a.ApplicationDate,
                    JobTitle = a.JobListing.Title,  // Ýlgili iþ ilanýnýn baþlýðý
                    a.JobListingId,                // Ýþ ilaný ID'si
                    JobSeekerId = a.JobSeeker.Id   // Ýþ arayanýn ID'si (Burada seçiliyor)
                })
                .ToList();

            return View(applications);
        }

        [HttpGet]
        public IActionResult Messages()
        {
            var employerId = HttpContext.Session.GetInt32("UserId");
            if (employerId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            // Ýþverene gelen mesajlarý çek
            var messages = _context.UserMessages
                .Where(m => m.ReceiverId == employerId)
                .Select(m => new
                {
                    m.SenderId,                             // Gönderen ID'si eklendi
                    SenderName = m.Sender.FirstName + " " + m.Sender.LastName,
                    JobTitle = m.JobListing.Title,
                    m.Message,
                    m.SentAt,
                    m.JobListingId                          // Ýlan ID'si eklendi
                })
                .OrderByDescending(m => m.SentAt)          // En son mesajlarý üstte göstermek için sýralama
                .ToList();

            return View(messages);
        }



        [HttpPost]
        public IActionResult SendMessage(int receiverId, int jobListingId, string message)
        {
            var senderId = HttpContext.Session.GetInt32("UserId"); // Giriþ yapan kullanýcý ID'sini alýyoruz
            if (senderId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            // ReceiverId'nin yanlýþlýkla senderId ile ayný olmasýný engellemek için kontrol edelim
            if (senderId.Value == receiverId)
            {
                return BadRequest("Kendinize mesaj gönderemezsiniz.");
            }

            // Mesaj oluþturma ve kaydetme
            var userMessage = new UserMessage
            {
                SenderId = senderId.Value, // Giriþ yapan iþverenin ID'si
                ReceiverId = receiverId,  // Ýþ arayanýn ID'si
                JobListingId = jobListingId, // Ýlgili iþ ilanýnýn ID'si
                Message = message,
                SentAt = DateTime.Now
            };

            _context.UserMessages.Add(userMessage);
            _context.SaveChanges();

            return RedirectToAction("Applications"); // Baþvuru sayfasýna geri dön
        }

        [HttpPost]
        public IActionResult ReplyMessage(int receiverId, int jobListingId, string message)
        {
            var senderId = HttpContext.Session.GetInt32("UserId");
            if (senderId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            // Yanýt olarak yeni mesaj oluþtur
            var replyMessage = new UserMessage
            {
                SenderId = senderId.Value, // Yanýt gönderen iþveren
                ReceiverId = receiverId,  // Yanýt alan iþ arayan
                JobListingId = jobListingId, // Ýlgili iþ ilaný
                Message = message,
                SentAt = DateTime.Now
            };

            _context.UserMessages.Add(replyMessage);
            _context.SaveChanges();

            return RedirectToAction("Messages"); // Mesajlar sayfasýna geri dön
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
