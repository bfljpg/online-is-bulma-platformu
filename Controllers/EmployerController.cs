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
                    ViewBag.UserName = "Bilinmeyen Kullan�c�";
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



        // Yeni ilan ekleme sayfas�
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

        // �lan sil
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

            // ��verene ait t�m ba�vurular� getir
            var applications = _context.JobApplications
                .Where(a => a.JobListing.EmployerId == employerId)
                .Select(a => new
                {
                    a.Id,
                    JobSeekerName = a.JobSeeker.FirstName + " " + a.JobSeeker.LastName,
                    a.Message,
                    a.ApplicationDate,
                    JobTitle = a.JobListing.Title,  // �lgili i� ilan�n�n ba�l���
                    a.JobListingId,                // �� ilan� ID'si
                    JobSeekerId = a.JobSeeker.Id   // �� arayan�n ID'si (Burada se�iliyor)
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

            // ��verene gelen mesajlar� �ek
            var messages = _context.UserMessages
                .Where(m => m.ReceiverId == employerId)
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
        public IActionResult SendMessage(int receiverId, int jobListingId, string message)
        {
            var senderId = HttpContext.Session.GetInt32("UserId"); // Giri� yapan kullan�c� ID'sini al�yoruz
            if (senderId == null)
            {
                return RedirectToAction("Login", "RoleBased");
            }

            // ReceiverId'nin yanl��l�kla senderId ile ayn� olmas�n� engellemek i�in kontrol edelim
            if (senderId.Value == receiverId)
            {
                return BadRequest("Kendinize mesaj g�nderemezsiniz.");
            }

            // Mesaj olu�turma ve kaydetme
            var userMessage = new UserMessage
            {
                SenderId = senderId.Value, // Giri� yapan i�verenin ID'si
                ReceiverId = receiverId,  // �� arayan�n ID'si
                JobListingId = jobListingId, // �lgili i� ilan�n�n ID'si
                Message = message,
                SentAt = DateTime.Now
            };

            _context.UserMessages.Add(userMessage);
            _context.SaveChanges();

            return RedirectToAction("Applications"); // Ba�vuru sayfas�na geri d�n
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
                SenderId = senderId.Value, // Yan�t g�nderen i�veren
                ReceiverId = receiverId,  // Yan�t alan i� arayan
                JobListingId = jobListingId, // �lgili i� ilan�
                Message = message,
                SentAt = DateTime.Now
            };

            _context.UserMessages.Add(replyMessage);
            _context.SaveChanges();

            return RedirectToAction("Messages"); // Mesajlar sayfas�na geri d�n
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
