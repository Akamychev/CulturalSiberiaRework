using System.Security.Cryptography;
using System.Text;

namespace CulturalSiberiaDiplom.Services;

public class Hash
{
    public static string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }
}