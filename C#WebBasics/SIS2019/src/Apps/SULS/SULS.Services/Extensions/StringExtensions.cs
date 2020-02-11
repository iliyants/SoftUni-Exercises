using System.Security.Cryptography;
using System.Text;

namespace SULS.Services.Extensions
{
    public static class StringExtensions
    {
        public static string HashPassword(this string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}
