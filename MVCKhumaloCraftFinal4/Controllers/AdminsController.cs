using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCKhumaloCraftFinal4.Data;
using MVCKhumaloCraftFinal4.Models;

namespace MVCKhumaloCraftFinal2.Controllers
{
    public class AdminsController : Controller
    {
        private readonly MVCKhumaloCraftFinal4Context _context;

        public AdminsController(MVCKhumaloCraftFinal4Context context)
        {
            _context = context;
        }

        // GET: Admins/Login
        public IActionResult Login()
        {
            return View("AdminLogin");
        }

        // POST: Admins/Login
        [HttpPost]
        public IActionResult Login(Admin model)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.adminEmail == model.adminEmail && a.adminPassword == model.adminPassword);
            if (admin != null)
            {
                // Setting the session variable to identify admin
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetString("AdminEmail", admin.adminEmail); // Storing admin email in session (for displaying admin email after log in)
                return RedirectToAction("Index", "Home");
            }

            // If login fails, adding an error message to ModelState
            ModelState.AddModelError(string.Empty, "Incorrect email or password.");
            return View("AdminLogin", model);
        }

        // POST: Admins/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Index", "Home"); // Redirect to home page
        }

        // GET: Admins/Dashboard
        public IActionResult Dashboard()
        {
            var orders = _context.Order.ToList();
            return View("Index", orders);
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admin.ToListAsync());
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.adminID == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("adminID,adminEmail,adminPassword")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("adminID,adminEmail,adminPassword")] Admin admin)
        {
            if (id != admin.adminID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.adminID))
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
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.adminID == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin != null)
            {
                _context.Admin.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.adminID == id);
        }
    }
}
