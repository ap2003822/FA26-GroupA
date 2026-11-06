using Microsoft.AspNetCore.Identity;

namespace InventoryApi.Models;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
}
