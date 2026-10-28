using System.ComponentModel.DataAnnotations;

namespace Alliance_Webapp.Models
{
    public class Item
    {
        [Key]
        public int Item_ID { get; set; }
        public string? Item_Name { get; set; }
        public string? Item_Image { get; set; }
        public int? Item_Unit_Amount { get; set; }
        public string? Item_Desc { get; set; }
        public DateTime? Item_Purchase_Date { get; set; }
        public DateTime? Item_Service_Date { get; set; }
        public int? Item_Serial_Number { get; set; }
        public string? Item_Notes { get; set; }
        public int? Item_Low_ALert_Number { get; set; }

    }
}
