namespace kütüphaneuygulaması.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        
        // Navigation property
        public ICollection<Book> Books { get; set; }
    }
} 