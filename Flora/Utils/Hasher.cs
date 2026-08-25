using System.Security.Cryptography;
namespace Flora.Utils
{



    public class Hasher
    {
        public static (string HashedPassword, string SaltBase64) HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 256 بیت
                return (
                    HashedPassword: Convert.ToBase64String(hash),
                    SaltBase64: Convert.ToBase64String(salt)
                );
            }
        }

        public static bool VerifyPassword(string password, string storedHashBase64, string storedSaltBase64)
        {
            byte[] salt = Convert.FromBase64String(storedSaltBase64);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                string computedHash = Convert.ToBase64String(hash);

                // مقایسه‌ی هش‌ها
                return computedHash == storedHashBase64;
            }
        }
    }

}