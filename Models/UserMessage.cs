namespace online_is_bulma_platformu.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int JobListingId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public virtual JobListing JobListing { get; set; }
    }
}
