using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kütüphaneuygulaması.Data;
using kütüphaneuygulaması.Models;

namespace kütüphaneuygulaması.Controllers
{
    public class ordersController : Controller
    {
        private readonly kütüphaneuygulamasıContext _context;

        public ordersController(kütüphaneuygulamasıContext context)
        {
            _context = context;
        }

        // GET: orders
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("myorders");
            var orders = await _context.orders.ToListAsync();
            var books = await _context.Book.ToListAsync();
            var totalSales = orders.Sum(o => (books.FirstOrDefault(b => b.Id == o.bookId)?.price ?? 0) * o.quantity);
            ViewBag.TotalSales = totalSales;
            return View(orders);
        }

        // GET: orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: orders/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userid")))
                return RedirectToAction("login", "usersaccounts");
            var book = await _context.Book.FindAsync(id);
            return View(book);
        }


        // POST: orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId, int quantity)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userid")))
                return RedirectToAction("login", "usersaccounts");
            orders order = new orders();
            order.bookId = bookId;
            order.quantity = quantity;
            order.userid = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            order.orderdate = DateTime.Today;
            _context.Add(order);
            await _context.SaveChangesAsync();
            // Müşteri ise kendi siparişlerine, admin ise tüm satışlara yönlendir
            if (HttpContext.Session.GetString("Role") == "admin")
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction("myorders");
        }

        public async Task<IActionResult> myorders()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var orders = await _context.orders.Where(o => o.userid == id).ToListAsync();
            var books = await _context.Book.ToListAsync();
            var enrichedOrders = orders.Select(o => new
            {
                o.Id,
                o.bookId,
                o.quantity,
                o.orderdate,
                BookTitle = books.FirstOrDefault(b => b.Id == o.bookId)?.title ?? "-",
                BookPrice = books.FirstOrDefault(b => b.Id == o.bookId)?.price ?? 0,
                Total = (books.FirstOrDefault(b => b.Id == o.bookId)?.price ?? 0) * o.quantity
            }).ToList();
            ViewBag.EnrichedOrders = enrichedOrders;
            return View(orders);
        }

        public async Task<IActionResult> customerOrders(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");


            var orItems = await _context.orders.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
            return View(orItems);

        }



        public async Task<IActionResult> customerreport()
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            var orItems = await _context.report.FromSqlRaw("select usersaccounts.id as Id, name as customername, sum (quantity * Price)  as total from book, orders,usersaccounts  where usersaccounts.id = orders.userid  and bookid= book.Id group by name,usersaccounts.id ").ToListAsync();
            return View(orItems);

        }



        // GET: orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,bookId,userid,quantity,orderdate")] orders orders)
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            if (id != orders.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ordersExists(orders.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "admin")
                return RedirectToAction("login", "usersaccounts");
            var orders = await _context.orders.FindAsync(id);
            if (orders != null)
            {
                _context.orders.Remove(orders);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ordersExists(int id)
        {
            return _context.orders.Any(e => e.Id == id);
        }
    }
}
