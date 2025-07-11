using kütüphaneuygulaması.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kütüphaneuygulaması.Models
{
    public class usersaccountsController : Controller
    {
        private readonly kütüphaneuygulamasıContext _context;

        public usersaccountsController(kütüphaneuygulamasıContext context)
        {
            _context = context;
        }

        // GET: usersaccounts
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            return View(await _context.usersaccounts.ToListAsync());
        }

    

        // GET: usersaccounts/Create
        public IActionResult Create()
        {
            return View();
        }


        //  usersaccounts/login
        public IActionResult login()
        {
            return View();
        }



        [HttpPost, ActionName("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(string na, string pa)
        {
            var user = await _context.usersaccounts.FirstOrDefaultAsync(u => u.name == na && u.pass == pa);
            if (user != null)
            {
                HttpContext.Session.SetString("Name", user.name);
                HttpContext.Session.SetString("Role", user.role);
                HttpContext.Session.SetString("userid", user.Id.ToString());
                if (user.role == "customer")
                    return RedirectToAction("catalogue", "books");
                else
                    return RedirectToAction("Index", "books");
            }
            ViewData["Message"] = "wrong user name password";
            return View();
        }




        // POST: usersaccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,pass,email")] usersaccounts usersaccounts)
        {
            usersaccounts.role = "customer";
            _context.Add(usersaccounts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(login));
        }

        // GET: usersaccounts/CreateAdmin
        public IActionResult CreateAdmin()
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("Id,name,pass,email")] usersaccounts usersaccounts)
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            usersaccounts.role = "admin";
            _context.Add(usersaccounts);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: usersaccounts/Edit/5
        public async Task<IActionResult> Edit()
        {
            // Kullanıcı sadece kendi profilini düzenleyebilir, admin ise herkesi düzenleyebilir
            if (HttpContext.Session.GetString("Role") != "admin" && HttpContext.Session.GetString("userid") == null)
                return RedirectToAction("login", "usersaccounts");
            int id = Convert.ToInt32(HttpContext.Session.GetString("userid"));

            var usersaccounts = await _context.usersaccounts.FindAsync(id);
          
            return View(usersaccounts);
        }

        // POST: usersaccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,pass,role,email")] usersaccounts usersaccounts)
        {
            // Kullanıcı sadece kendi profilini düzenleyebilir, admin ise herkesi düzenleyebilir
            if (HttpContext.Session.GetString("Role") != "admin" && HttpContext.Session.GetString("userid") != id.ToString())
                return RedirectToAction("login", "usersaccounts");
                    _context.Update(usersaccounts);
                    await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(login));
          
        }

      
        private bool usersaccountsExists(int id)
        {
            return _context.usersaccounts.Any(e => e.Id == id);
        }
    }
}
