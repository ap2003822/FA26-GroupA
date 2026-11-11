using AllianceInventoryApi.Models;
using Microsoft.AspNetCore.Identity;

namespace AllianceInventoryApi;

public static class PasswordHelper
{
    private static readonly PasswordHasher<UserModel> Hasher = new();

    public static string Hash(UserModel user, string password) =>
        Hasher.HashPassword(user, password);

    public static bool Verify(UserModel user, string password)
    {
        if (string.IsNullOrEmpty(user.User_Password)) return false;
        try
        {
            return Hasher.VerifyHashedPassword(user, user.User_Password, password)
                   != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            return false;   // the stored value isn't a hash (still plain text)
        }
    }
}