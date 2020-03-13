using System;

namespace InTechNet.Common.Utils.Authentication.Jwt.Models
{
    public class JwtResourceHelper
    {
        public const string AppSettingsSectionName = "JwtToken";

        public string Audience { get; set; }

        public string Issuer { get; set; }
        
        public string SecretKey { get; set; }

        // TODO: cste in this DTO
        public DateTime ValidUntil
            => DateTime.Now.AddDays(1);
    }
}
