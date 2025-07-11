using Microsoft.AspNetCore.Mvc;
using kütüphaneuygulaması.Data;
using kütüphaneuygulaması.Models;
using Microsoft.EntityFrameworkCore;

namespace kütüphaneuygulaması.Controllers
{
    public class CartController : Controller
    {
        private readonly kütüphaneuygulamasıContext _context;
        public CartController(kütüphaneuygulamasıContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var cartItems = await _context.Cart.Where(c => c.UserId == userId)
                .Include(c => c.Book)
                .ToListAsync();
            var books = await _context.Book.ToListAsync();
            var enrichedCart = cartItems.Select(c => new
            {
                c.Id,
                c.BookId,
                BookTitle = books.FirstOrDefault(b => b.Id == c.BookId)?.title ?? "-",
                BookPrice = books.FirstOrDefault(b => b.Id == c.BookId)?.price ?? 0,
                c.Quantity,
                Total = (books.FirstOrDefault(b => b.Id == c.BookId)?.price ?? 0) * c.Quantity
            }).ToList();
            ViewBag.EnrichedCart = enrichedCart;
            return View();
        }

        // POST: Cart/Add
        [HttpPost]
        public async Task<IActionResult> Add(int bookId)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var cartItem = await _context.Cart.FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                _context.Update(cartItem);
            }
            else
            {
                _context.Cart.Add(new Cart
                {
                    UserId = userId,
                    BookId = bookId,
                    Quantity = 1,
                    AddedDate = DateTime.Now
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Cart/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var cartItem = await _context.Cart.FindAsync(id);
            if (cartItem != null)
            {
                _context.Cart.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // POST: Cart/Checkout
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var cartItems = await _context.Cart.Where(c => c.UserId == userId).ToListAsync();
            foreach (var item in cartItems)
            {
                _context.orders.Add(new orders
                {
                    bookId = item.BookId,
                    userid = userId,
                    quantity = item.Quantity,
                    orderdate = DateTime.Now
                });
            }
            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return RedirectToAction("myorders", "orders");
        }
    }
} 