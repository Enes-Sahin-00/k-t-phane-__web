﻿namespace kütüphaneuygulaması.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string info { get; set; }
        public int bookquantity { get; set; }
        public int price { get; set; }
        public int cataid { get; set; }
        public string author { get; set; }
        public string imgfile { get; set; }

        // Navigation properties
        public Category Category { get; set; }
    }
}
