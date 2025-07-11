namespace kütüphaneuygulaması.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }

        // Navigation property
        public Book Book { get; set; }
    }
} 