using System.Security.Cryptography;
using System.Text;

namespace ShopServer.Helpers.HashOperation.PassEncrypter
{
    public static class PasswordEncrypter
    {
        public static string Encrypt(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedByters = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedByters);
        }
    }
}
