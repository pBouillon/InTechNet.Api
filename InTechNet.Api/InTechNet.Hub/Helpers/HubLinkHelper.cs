using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace InTechNet.Service.Hub.Helpers
{
    public static class HubLinkHelper
    {
        /// <summary>
        /// Hash a hub name with the given salt using <see cref="SHA512Managed" />
        /// </summary>
        /// <param name="hub"><see cref="HubDto" /> for the name to hash</param>
        /// <param name="moderator"><see cref="ModeratorDto" /> for the salt</param>
        /// <returns>The Hub specific link</returns>
        public static string GenerateLink(HubDto hub, ModeratorDto moderator)
        {
            var toHash = Encoding.UTF8.GetBytes(hub.Name + moderator.Id + moderator.Nickname);

            using var sha256Managed = new SHA256Managed();

            return WebEncoders.Base64UrlEncode(
                sha256Managed.ComputeHash(toHash));
        }
    }
}
