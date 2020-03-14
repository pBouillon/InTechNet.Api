using System;
using System.Text;

namespace InTechNet.Service.User.Helper
{
    public static class UserPasswordHelper
    {
        public static string HashedWith(this string password, string salt)
        {
            var toHash = Encoding.UTF8.GetBytes(password + salt);

            using var sha512Managed = new System.Security.Cryptography.SHA512Managed();

            return BitConverter
                .ToString(sha512Managed.ComputeHash(toHash))
                .Replace("-", string.Empty);
        }
    }
}
