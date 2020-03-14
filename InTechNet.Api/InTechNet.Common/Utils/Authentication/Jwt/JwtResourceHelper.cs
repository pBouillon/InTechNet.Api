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
            => SecretKey != null ?
                Encoding.UTF8.GetBytes(SecretKey)
                : new byte[0];

        public string Issuer { get; set; }
        
        public string SecretKey { get; set; }

        public string SigningAlgorithm 
            => SecurityAlgorithms.HmacSha512;

        public int ValidityTimespanInHours { get; set; }

        public DateTime ValidUntil
            => DateTime.Now.AddHours(ValidityTimespanInHours);
    }
}
