using System.ComponentModel.DataAnnotations;

namespace AllianceInventoryApi.Models
{
    public class AddCatModel
    {
        [Required]
        public string Item_Name { get; set; }
        
        public string? Item_Image { get; set; }
        public int? Item_Unit_Amount { get; set; }
        [Required]
        public string Item_Desc { get; set; }
        public DateTime? Item_Purchase_Date { get; set; }
        public DateTime? Item_Service_Date { get; set; }
        [Required]
        public int Item_Serial_Number { get; set; }
        public string? Item_Notes { get; set; }
        [Required]
        public int Item_Low_ALert_Number { get; set; }
    }
}
