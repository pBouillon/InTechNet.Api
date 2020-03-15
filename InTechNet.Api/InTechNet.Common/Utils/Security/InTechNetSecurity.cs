using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace InTechNet.Common.Utils.Security
{
    public class InTechNetSecurity
    {
        /// <summary>
        /// Hashing algorithm used for JWT signature
        /// </summary>
        public const string JwtSigningAlgorithm = SecurityAlgorithms.HmacSha512;

        /// <summary>
        /// Size of the byte array for the salt generation
        /// </summary>
        public const int SaltByteLength = 128;

        /// <summary>
        /// Get a randomly generated salt
        /// </summary>
        /// <returns>The generated salt</returns>
        public static string GetSalt()
        {
            var saltBuffer = new byte[SaltByteLength];

            using var cryptoServiceProvider = new RNGCryptoServiceProvider();
            cryptoServiceProvider.GetNonZeroBytes(saltBuffer);

            return Convert.ToBase64String(saltBuffer);
        }
    }
}
