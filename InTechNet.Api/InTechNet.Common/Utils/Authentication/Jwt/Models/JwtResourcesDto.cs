using System;

namespace InTechNet.Common.Utils.Authentication.Jwt.Models
{
    public class JwtResourcesDto
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }
        
        public string SecretKey { get; set; }

        public DateTime ValidUntil
            => DateTime.Now.AddDays(1);
    }
}
