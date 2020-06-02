using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InTechNet.Services.User.Helpers
{
    /// <summary>
    /// Toolbox for the user password
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hash a password with the given salt using <see cref="SHA512Managed()" />
        /// </summary>
        /// <param name="password">Raw password to hash</param>
        /// <param name="salt">Salt to use along with the password</param>
        /// <returns>The hashed password</returns>
        public static string HashedWith(this string password, string salt)
        {
            var toHash = Encoding.UTF8.GetBytes(password + salt);

            using var sha512Managed = new SHA512Managed();

            return BitConverter
                .ToString(sha512Managed.ComputeHash(toHash))
                .Replace("-", string.Empty);
        }

        /// <summary>
        /// Assert that the password is strong enough based on a set of specifications
        /// </summary>
        /// <param name="password">The password to be checked</param>
        /// <returns>True if the password is considered secure enough; false otherwise</returns>
        public static bool IsStrongEnough(string password)
        {
            return password.Length < 8
                   || password.Length > 64
                   || !password.Any(char.IsUpper)
                   || !password.Any(char.IsLower)
                   || !password.Any(char.IsDigit);
        }
    }
}
