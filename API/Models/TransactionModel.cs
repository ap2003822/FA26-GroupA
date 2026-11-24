using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllianceInventoryApi.Models
{
    public class TransactionModel
    {
        [Key]
        public int Transaction_Id { get; set; }
        public int Item_Id { get; set; }
        public int User_Id { get; set; }
        public string Transaction_Type { get; set; }
        public DateTime Transaction_Date { get; set; }
        public string Change_Desc { get; set; }
        public string Old_Value { get; set; }
        public string New_Value { get; set; }
        [NotMapped]
        public string? User_Email { get; set; }
        [NotMapped]
        public string? Item_Name { get; set; }
    }
}
