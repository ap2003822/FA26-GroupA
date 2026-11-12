using System.ComponentModel.DataAnnotations;

namespace Alliance_Webapp.Models
{
    public class UserModel
    {
        [Key]
        public int User_ID { get; set; }
        public string? User_Email { get; set; }
        public string? User_Password { get; set; }
        public string? User_Role { get; set; }
    }
}
