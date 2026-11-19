using System.ComponentModel.DataAnnotations;

namespace AllianceInventoryApi.API.Models
{
    public class Edits
    {
        public int Edit_ID { get; set; }

        public int Item_ID { get; set; }

        public int? User_ID { get; set; }

        public string Transaction_Type { get; set; }

        public DateTime Transaction_Date { get; set; }

        public string Change_Desc { get; set; }

        public string? Old_Value { get; set; }

        public string? New_Value { get; set; }
    }
}