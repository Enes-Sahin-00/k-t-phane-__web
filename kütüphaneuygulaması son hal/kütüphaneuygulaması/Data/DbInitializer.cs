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

            // Kategoriler ekle
            if (!context.Category.Any())
            {
                context.Category.AddRange(
                    new Category { Name = "Bilgisayar Bilimi", Description = "Bilgisayar bilimi ve programlama kitapları", ImageUrl = "computer_science.jpg" },
                    new Category { Name = "Bilgisayar Mühendisliği", Description = "Bilgisayar mühendisliği ve teknoloji kitapları", ImageUrl = "computer_engineering.jpg" },
                    new Category { Name = "Roman", Description = "Edebiyat ve roman kitapları", ImageUrl = "novel.jpg" },
                    new Category { Name = "Tarih", Description = "Tarih ve sosyal bilimler kitapları", ImageUrl = "history.jpg" },
                    new Category { Name = "Bilim", Description = "Bilim ve araştırma kitapları", ImageUrl = "science.jpg" }
                );
                context.SaveChanges();
            }

            // Örnek kitap ekle
            if (!context.Book.Any())
            {
                context.Book.AddRange(
                    new Book
                    {
                        title = "C# Programlama",
                        info = "C# programlama dili ile ilgili kapsamlı rehber.",
                        bookquantity = 10,
                        price = 150,
                        cataid = 1,
                        author = "Ahmet Yılmaz",
                        imgfile = "csharp_book.jpg"
                    },
                    new Book
                    {
                        title = "Veri Yapıları ve Algoritmalar",
                        info = "Veri yapıları ve algoritma analizi.",
                        bookquantity = 8,
                        price = 200,
                        cataid = 1,
                        author = "Mehmet Demir",
                        imgfile = "data_structures.jpg"
                    },
                    new Book
                    {
                        title = "Web Geliştirme",
                        info = "Modern web geliştirme teknikleri.",
                        bookquantity = 12,
                        price = 180,
                        cataid = 2,
                        author = "Fatma Kaya",
                        imgfile = "web_development.jpg"
                    }
                );
                context.SaveChanges();
            }
        }
    }
} 