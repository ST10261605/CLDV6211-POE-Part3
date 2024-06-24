using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCKhumaloCraftFinal4.Data;
using MVCKhumaloCraftFinal4.Models;

namespace MVCKhumaloCraft2.Controllers
{
    public class UsersController : Controller
    {
        private readonly MVCKhumaloCraftFinal4Context _context;

        public UsersController(MVCKhumaloCraftFinal4Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.userID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["IsRegisterPage"] = true;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userID,Name,Surname,Password,Email, Enable2FA, TwoFactorCode, TwoFactorCodeExpiration")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Users");
            }
            return View(user);
        }

        // GET: Home/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    if (user.Enable2FA)
                    {
                        // Generate a new 2FA code
                        user.TwoFactorCode = new Random().Next(100000, 999999).ToString();
                        user.TwoFactorCodeExpiration = DateTime.UtcNow.AddMinutes(10); // 10 minutes expiration
                        await _context.SaveChangesAsync();

                        // Store UserId in a cookie
                        Response.Cookies.Append("userID", user.userID.ToString(), new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddHours(1),
                            HttpOnly = true,
                            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                            Secure = true // Ensure to set to true if using HTTPS
                        });

                        // Generate the mailto link
                        var mailtoLink = GenerateMailtoLink(user.Email, user.TwoFactorCode);

                        ViewBag.MailtoLink = mailtoLink;
                        ViewBag.Email = user.Email;

                        return View("SendTwoFactorCode"); // Redirect to the SendTwoFactorCode view
                    }
                    else
                    {
                        // Proceed with regular login
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("userID", user.userID.ToString()) // Add userID claim
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        // Set userID cookie
                        Response.Cookies.Append("userID", user.userID.ToString());

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(model);
        }

        private string GenerateMailtoLink(string email, string code)
        {
            var subject = Uri.EscapeDataString("Your 2FA Code");
            var body = Uri.EscapeDataString($"Your 2FA code is {code}");
            return $"mailto:{email}?subject={subject}&body={body}";
        }

        // GET: Users/SendTwoFactorCode
        public IActionResult SendTwoFactorCode()
        {
            // Assuming you have set ViewBag.MailtoLink in your Login action
            return View();
        }

        // POST: Users/VerifyTwoFactorCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyTwoFactorCode(VerifyTwoFactorCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user ID from the cookie
                if (Request.Cookies.TryGetValue("userID", out string userIdStr) && int.TryParse(userIdStr, out int userId))
                {
                    // Retrieve the user from the database
                    var user = await _context.User.FindAsync(userId);

                    if (user != null && user.TwoFactorCode == model.TwoFactorCode && user.TwoFactorCodeExpiration > DateTime.UtcNow)
                    {
                        // Clear the 2FA properties after successful verification
                        user.TwoFactorCode = null;
                        user.TwoFactorCodeExpiration = null;
                        await _context.SaveChangesAsync();

                        // Proceed with signing in the user
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("userID", user.userID.ToString()) // Add userID claim
                    // Add any other claims as needed
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        // Set userID cookie again (in case it was removed during the redirect)
                        Response.Cookies.Append("userID", user.userID.ToString());

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid 2FA code or code has expired.");
            }

            // If we reach here, something went wrong, so we need to show the same view again with errors
            return View("SendTwoFactorCode", model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("userID,Name,Surname,Password,Email")] User user)
        {
            if (id != user.userID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.userID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.userID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.userID == id);
        }
    }
}