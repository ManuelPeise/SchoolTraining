using System.Text;

namespace Logic.Shared.Helpers
{
    public static class SecretHelper
    {
        public static string GetHashedSecret(string? password, string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            var passwordBytes = Encoding.UTF8.GetBytes(password).ToList();
            passwordBytes.AddRange(Encoding.UTF8.GetBytes(salt));

            return Convert.ToBase64String(passwordBytes.ToArray());
        }

        public static string GetPasswordHash(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password).ToList();
            bytes.AddRange(Encoding.UTF8.GetBytes(salt));

            return Convert.ToBase64String(bytes.ToArray());
        }
    }
}
