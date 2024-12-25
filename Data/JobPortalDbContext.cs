using System.Data.Entity;
using online_is_bulma_platformu.Models;

namespace online_is_bulma_platformu.Data
{
    public class JobPortalContext : DbContext
    {
        // Bağlantı dizesi doğrudan buraya yazılıyor
        public JobPortalContext() : base("Server=DESKTOP-SBTPIC1\\SQLEXPRESS;Database=JobPortalDB;Trusted_Connection=True;") { }

        public DbSet<User> Users { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
    }
}
