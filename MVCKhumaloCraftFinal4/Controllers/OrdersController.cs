using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCKhumaloCraftFinal4.Data;
using MVCKhumaloCraftFinal4.Models;

namespace MVCKhumaloCraftFinal4.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MVCKhumaloCraftFinal4Context _context;

        public OrdersController(MVCKhumaloCraftFinal4Context context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            bool isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            if (isAdmin)
            {
                // Admin logic: Retrieve all orders with the status "Processing"
                var allProcessingOrders = await _context.Order.Include(o => o.Product)
                                                              .Where(o => o.orderStatus == "Processing")
                                                              .ToListAsync();
                return View(allProcessingOrders);
            }
            else
            {
                // Regular user logic: Retrieve the user ID from the cookie
                if (!HttpContext.Request.Cookies.TryGetValue("userID", out string userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized();
                }

                // Get the user's orders
                var userOrders = await _context.Order.Include(o => o.Product)
                                                     .Where(o => o.userID == userId)
                                                     .ToListAsync();

                return View(userOrders);
            }
        }


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.orderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["productID"] = new SelectList(_context.Product, "productID", "productID");
            ViewData["userID"] = new SelectList(_context.User, "userID", "userID");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("orderID,userID,productID,deliveryCountry,shippingMethod,shippingAddress,phoneNumber, orderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(order.orderStatus))
                {
                    order.orderStatus = "Processing"; // Set default value of order status
                }

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productID"] = new SelectList(_context.Product, "productID", "productID", order.productID);
            ViewData["userID"] = new SelectList(_context.User, "userID", "userID", order.userID);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.orderStatus = "Processed";
            _context.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["productID"] = new SelectList(_context.Product, "productID", "productID", order.productID);
            ViewData["userID"] = new SelectList(_context.User, "userID", "userID", order.userID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("orderID,userID,productID,deliveryCountry,shippingMethod,shippingAddress,phoneNumber")] Order order)
        {
            if (id != order.orderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.orderID))
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
            ViewData["productID"] = new SelectList(_context.Product, "productID", "productID", order.productID);
            ViewData["userID"] = new SelectList(_context.User, "userID", "userID", order.userID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.orderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.orderID == id);
        }
    }
}
