using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MVCKhumaloCraftFinal4.Data;
using MVCKhumaloCraftFinal4.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace MVCKhumaloCraftFinal4.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MVCKhumaloCraftFinal4Context _context;

        public ProductsController(MVCKhumaloCraftFinal4Context context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.productID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create //only admins can create products
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") //if they are not an admin
            {
                return Forbid(); //do not allow them to create a new product
            }

            return View(); //show the products view
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("productID,productName, description, Price,Category,stockAvailable,ImageUrl")] Product product)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("productID,productName, description, Price,Category,stockAvailable,ImageUrl")] Product product)
        {
            if (id != product.productID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.productID))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Forbid();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.productID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Forbid();
            }

            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.productID == id);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            // Retrieve the user ID from the cookie
            if (!HttpContext.Request.Cookies.TryGetValue("userID", out string userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var product = await _context.Product.FirstOrDefaultAsync(b => b.productID == productId);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the item is already in the cart for the user
            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.ProductID == productId && c.userID == userId);

            if (cartItem == null)
            {
                // Item is not in the cart, add it
                cartItem = new CartItem
                {
                    ProductID = productId,
                    userID = userId,
                    Quantity = 1
                };
                _context.CartItem.Add(cartItem);
            }
            else
            {
                // Item is already in the cart, update the quantity
                cartItem.Quantity++;
                _context.CartItem.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
