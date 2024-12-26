namespace online_is_bulma_platformu.Models
{
    public class JobListing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EmployerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }

        public virtual User Employer { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; } // İlişki eklendi
    }
}
