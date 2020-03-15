using System;
using System.Text;

namespace InTechNet.Common.Utils.Authentication.Jwt
{
    /// <summary>
    /// References all valuable data for JWT generation purposes
    /// </summary>
    public class JwtResourceHelper
    {
        /// <summary>
        /// appsettings section from with we should fetch those data
        /// </summary>
        public const string AppSettingsSectionName = "JwtToken";

        /// <summary>
        /// JWT Audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Get the encoded secret key in UTF-8
        /// </summary>
        public byte[] EncodedSecretKey
            => SecretKey != null
                ? Encoding.UTF8.GetBytes(SecretKey)
                : new byte[0];

        /// <summary>
        /// JWT Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// JWT Secret key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Hours to be passed before invalidating a newly created JWT
        /// </summary>
        public int ValidityTimespanInHours { get; set; }

        /// <summary>
        /// <see cref="DateTime" /> on which the JWT to be generated will be outdated
        /// </summary>
        public DateTime ValidUntil
            => DateTime.Now.AddHours(ValidityTimespanInHours);
    }
}
