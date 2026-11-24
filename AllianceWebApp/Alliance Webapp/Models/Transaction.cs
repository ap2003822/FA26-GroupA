using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alliance_Webapp.Models
{
    public class Transaction
    {
        public int Transaction_Id { get; set; }
        public int Item_Id { get; set; }
        public int User_Id { get; set; }
        public string transaction_Type { get; set; }
        public DateTime Transaction_Date { get; set; }
        public string Change_Desc { get; set; }
        public string Old_Value { get; set; }
        public string New_Value { get; set; }
        public string? User_Email { get; set; }
        public string? Item_Name { get; set; }

    }
}
