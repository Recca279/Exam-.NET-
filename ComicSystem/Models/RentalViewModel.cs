using System;
using System.ComponentModel.DataAnnotations;

namespace ComicSystem.Models
{
    public class RentalViewModel
    {
        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int ComicBookID { get; set; }

        [Required]
        public DateTime RentalDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(7);

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public string Status { get; set; } = "Rented";
    }
}
