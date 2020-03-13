using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace InTechNet.Common.Utils.Authentication.Jwt.Models
{
    public class JwtResourceHelper
    {
        public const string AppSettingsSectionName = "JwtToken";

        public string Audience { get; set; }

        public byte[] EncodedSecretKey
            => Encoding.UTF8.GetBytes(SecretKey);

        public string Issuer { get; set; }
        
        public string SecretKey { get; set; }

        public string SigningAlgorithm
            => SecurityAlgorithms.HmacSha256;

        public DateTime ValidUntil
            => DateTime.Now.AddDays(1);
    }
}
