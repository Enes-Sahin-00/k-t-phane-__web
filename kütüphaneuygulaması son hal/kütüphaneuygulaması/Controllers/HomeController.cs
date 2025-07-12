using System.Diagnostics;
using kütüphaneuygulaması.Models;
using Microsoft.AspNetCore.Mvc;

namespace kütüphaneuygulaması.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["WelcomeMessage"] = "Kütüphane Uygulamasına Hoş Geldiniz!";
            ViewBag.CurrentDate = DateTime.Now;
            TempData["VisitCount"] = 1; // Bu gerçek uygulamada session'dan alınabilir
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "Hakkımızda";
            ViewBag.CompanyName = "Kütüphane Uygulaması";
            ViewBag.FoundedYear = 2024;
            ViewBag.Description = "Modern ve kullanıcı dostu kütüphane yönetim sistemi.";
            TempData["AboutMessage"] = "Bilgiye erişim herkesin hakkıdır.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "Bize Ulaşın";
            ViewBag.Email = "info@kutuphane.com";
            ViewBag.Phone = "+90 212 555 0123";
            ViewBag.Address = "İstanbul, Türkiye";
            TempData["ContactMessage"] = "Sorularınız için bizimle iletişime geçin.";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
