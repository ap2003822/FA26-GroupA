using System.ComponentModel.DataAnnotations;

namespace Alliance_Webapp.Models;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
    public string? ErrorMessage { get; set; }
}