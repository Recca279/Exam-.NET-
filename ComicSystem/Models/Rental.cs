using System.ComponentModel.DataAnnotations;

namespace ComicSystem.Models;

public class Rental
{
    [Key]
    public int RentalID { get; set; }

    public int CustomerID { get; set; }

    public DateTime RentalDate { get; set; }

    public DateTime ReturnDate { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    public Customer? Customer { get; set; }

    public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
}