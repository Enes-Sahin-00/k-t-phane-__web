using kütüphaneuygulaması.Models;

namespace kütüphaneuygulaması.Data
{
    public static class DbInitializer
    {
        public static void Initialize(kütüphaneuygulamasıContext context)
        {
            context.Database.EnsureCreated();

            // Admin var mı kontrol et
            if (!context.usersaccounts.Any(u => u.role == "admin"))
            {
                context.usersaccounts.Add(new usersaccounts
                {
                    name = "admin",
                    pass = "admin123", // Gerçek projede hash'lenmeli!
                    email = "admin@admin.com",
                    role = "admin"
                });
                context.SaveChanges();
            }

            // Örnek kitap ekle
            if (!context.Book.Any())
            {
                context.Book.Add(new Book
                {
                    title = "Örnek Kitap",
                    info = "Seed ile eklenmiş örnek kitap.",
                    bookquantity = 10,
                    price = 100,
                    cataid = 1,
                    author = "Yazar Seed"
                });
                context.SaveChanges();
            }
        }
    }
} 