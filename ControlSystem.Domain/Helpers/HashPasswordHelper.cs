using System.Security.Cryptography;
using System.Text;

namespace ControlSystem.Domain.Helpers
{
    public static class HashPasswordHelper
    {
        public static string HashPassword(string password)
        {
            byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace(".", "").ToLower();
            return hashedPassword;
        }
    }
}
