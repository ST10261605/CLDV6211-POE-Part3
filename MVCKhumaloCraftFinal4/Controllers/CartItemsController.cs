using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCKhumaloCraftFinal4.Data;
using MVCKhumaloCraftFinal4.Models;

namespace MVCKhumaloCraftFinal4.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly MVCKhumaloCraftFinal4Context _context;

        public CartItemsController(MVCKhumaloCraftFinal4Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve the user ID from the cookie
            if (!HttpContext.Request.Cookies.TryGetValue("userID", out string userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var cartItems = await _context.CartItem.Include(c => c.Product)
                                                   .Where(c => c.userID == userId)
                                                   .ToListAsync();
            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.ProductID == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductID = productId,
                    Product = product,
                    Quantity = 1
                };
                _context.CartItem.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Product added to cart!";
            return RedirectToAction("Index", "CartItems");
        }

        //method to update the quantity of the items in the cart
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItem.FindAsync(cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            // Retrieve the user ID from the cookie
            if (!HttpContext.Request.Cookies.TryGetValue("userID", out string userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var cartItems = await _context.CartItem.Include(c => c.Product)
                                                   .Where(c => c.userID == userId)
                                                   .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {

                return RedirectToAction("Index");
            }

            var viewModel = new OrderViewModel
            {
                CartItems = cartItems,
                deliveryCountry = string.Empty, // Initialize with default values if needed
                shippingMethod = string.Empty,
                shippingAddress = string.Empty,
                phoneNumber = string.Empty
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckout(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user ID from the cookie
                if (!HttpContext.Request.Cookies.TryGetValue("userID", out string userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized();
                }

                // Retrieve cart items for the user
                var cartItems = await _context.CartItem.Include(c => c.Product)
                                                       .Where(c => c.userID == userId)
                                                       .ToListAsync();

                if (cartItems == null || !cartItems.Any())
                {
                    ModelState.AddModelError(string.Empty, "No items in the cart.");
                    model.CartItems = cartItems;
                    return View("Checkout", model);
                }

                // Save each cart item as an order
                foreach (var cartItem in cartItems)
                {
                    var order = new Order
                    {
                        userID = userId,
                        productID = cartItem.ProductID,
                        deliveryCountry = model.deliveryCountry,
                        shippingMethod = model.shippingMethod,
                        shippingAddress = model.shippingAddress,
                        phoneNumber = model.phoneNumber
                    };

                    _context.Order.Add(order);
                    _context.CartItem.Remove(cartItem); // Remove items from the cart after checkout
                }

                await _context.SaveChangesAsync();

                // Redirect to the Orders page after successful checkout
                return RedirectToAction("Index", "Orders");
            }

            // If model validation fails, retrieve cart items again and pass them to the view
            if (model.CartItems == null)
            {
                // Retrieve the user ID from the cookie
                if (!HttpContext.Request.Cookies.TryGetValue("userID", out string userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized();
                }

                // Retrieve cart items for the user
                var cartItems = await _context.CartItem.Include(c => c.Product)
                                                       .Where(c => c.userID == userId)
                                                       .ToListAsync();

                model.CartItems = cartItems;
            }

            return View("Checkout", model);
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemID == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "productID", "productID");
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartItemID,ProductID,Quantity")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "productID", "productID", cartItem.ProductID);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "productID", "productID", cartItem.ProductID);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartItemID,ProductID,Quantity")] CartItem cartItem)
        {
            if (id != cartItem.CartItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.CartItemID))
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
            ViewData["ProductID"] = new SelectList(_context.Product, "productID", "productID", cartItem.ProductID);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemID == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.CartItemID == id);
        }
    }
}
