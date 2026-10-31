using System.ComponentModel.DataAnnotations;

namespace AllianceInventoryApi.Models
{
    public class UserModel
    {
        [Key]
        public int User_id { get; set; }
        public string User_Email { get; set; }
        public string User_Password { get; set; }
        public string User_Role { get; set; }
    }
}
