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
        public const int SaltByteLength = 256;
    }
}
