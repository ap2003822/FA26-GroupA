using Microsoft.AspNetCore.Identity;

namespace AllianceInventoryApi.Models;

public class AuthorizedUserModel : IdentityUser
{
    public string? Name { get; set; }
}
