using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicSystem.Data;
using ComicSystem.Models;

namespace ComicSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Customers/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("CustomerID,FullName,PhoneNumber,RegistrationDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.RegistrationDate = DateTime.Now; // Automatically set to now or let user input it? The requirement says "Register date". Usually this is automatic, but we can accept input if provided, or override it.
                // It's better to just use what the user submitted, or set it if empty. Let's let the form have the date input, defaulting to today.
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}
