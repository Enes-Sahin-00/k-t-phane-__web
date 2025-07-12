using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kütüphaneuygulaması.Models;

namespace kütüphaneuygulaması.Data
{
    public class kütüphaneuygulamasıContext : DbContext
    {
        public kütüphaneuygulamasıContext (DbContextOptions<kütüphaneuygulamasıContext> options)
            : base(options)
        {
        }

        public DbSet<kütüphaneuygulaması.Models.Book> Book { get; set; } = default!;
        public DbSet<kütüphaneuygulaması.Models.usersaccounts> usersaccounts { get; set; } = default!;
        public DbSet<kütüphaneuygulaması.Models.orders> orders { get; set; } = default!;
        public DbSet<kütüphaneuygulaması.Models.report> report { get; set; } = default!;
        public DbSet<kütüphaneuygulaması.Models.Cart> Cart { get; set; } = default!;
        public DbSet<kütüphaneuygulaması.Models.Category> Category { get; set; } = default!;
        public DbSet<kütüphaneuygulaması.Models.Favorite> Favorite { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Book - Category relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.cataid);

            // Favorite relationships
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Book)
                .WithMany()
                .HasForeignKey(f => f.BookId);
        }
    }
}
