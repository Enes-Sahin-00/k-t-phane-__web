namespace kütüphaneuygulaması.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime AddedDate { get; set; }

        // Navigation properties
        public usersaccounts User { get; set; }
        public Book Book { get; set; }
    }
} 