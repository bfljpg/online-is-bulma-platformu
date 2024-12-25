namespace online_is_bulma_platformu.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobListingId { get; set; }
        public int JobSeekerId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Message { get; set; }

        public virtual JobListing JobListing { get; set; }
        public virtual User JobSeeker { get; set; }
    }
}
