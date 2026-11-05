using System.ComponentModel.DataAnnotations;

namespace AllianceInventoryApi.API.Models
{
    public class Edit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string ItemName { get; set; }

        [Required]
        public decimal ItemUnitAmount { get; set; }

        public string? ItemImage { get; set; }

        [MaxLength(2000)]
        public string? ItemDesc { get; set; }

        public DateTime? ItemPurchaseDate { get; set; }

        public DateTime? ItemServiceDate { get; set; }

        [MaxLength(100)]
        public string? ItemSerialNumber { get; set; }

        [MaxLength(2000)]
        public string? ItemNotes { get; set; }
    }
}