using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComicSystem.Data;
using ComicSystem.Models;
using System.Collections.Generic;

namespace ComicSystem.Controllers
{
    public class RentalsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
            ViewData["ComicBookID"] = new SelectList(_context.ComicBooks, "ComicBookID", "Title");
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comicBook = await _context.ComicBooks.FindAsync(model.ComicBookID);
                if (comicBook == null)
                {
                    return NotFound();
                }

                var rental = new Rental
                {
                    CustomerID = model.CustomerID,
                    RentalDate = model.RentalDate,
                    ReturnDate = model.ReturnDate,
                    Status = model.Status
                };

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync(); // To get RentalID

                var rentalDetail = new RentalDetail
                {
                    RentalID = rental.RentalID,
                    ComicBookID = model.ComicBookID,
                    Quantity = model.Quantity,
                    PricePerDay = comicBook.PricePerDay
                };

                _context.RentalDetails.Add(rentalDetail);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Report)); // Redirect to report or list
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", model.CustomerID);
            ViewData["ComicBookID"] = new SelectList(_context.ComicBooks, "ComicBookID", "Title", model.ComicBookID);
            return View(model);
        }

        // GET: Rentals/Report
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.RentalDetails
                .Include(rd => rd.Rental)
                    .ThenInclude(r => r.Customer)
                .Include(rd => rd.ComicBook)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(rd => rd.Rental.RentalDate >= startDate.Value);
            }
            
            if (endDate.HasValue)
            {
                query = query.Where(rd => rd.Rental.RentalDate <= endDate.Value);
            }

            var result = await query.Select(rd => new ReportViewModel
            {
                BookName = rd.ComicBook.Title,
                RentalDate = rd.Rental.RentalDate,
                ReturnDate = rd.Rental.ReturnDate,
                CustomerName = rd.Rental.Customer.FullName,
                Quantity = rd.Quantity
            }).ToListAsync();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(result);
        }
    }

    public class ReportViewModel
    {
        public string BookName { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string CustomerName { get; set; }
        public int Quantity { get; set; }
    }
}
