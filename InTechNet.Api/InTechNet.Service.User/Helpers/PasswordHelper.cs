using System;
using System.Security.Cryptography;
using System.Text;

namespace InTechNet.Service.User.Helpers
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
    }
}
