using Microsoft.EntityFrameworkCore;
using online_is_bulma_platformu.Models;

namespace online_is_bulma_platformu.Data
{
    public class JobPortalContext : DbContext
    {
        public JobPortalContext(DbContextOptions<JobPortalContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
    }
}
