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
        /// Generate a unique identifier for a hub given its owner
        /// </summary>
        /// <param name="hub">The <see cref="HubCreationDto" /> containing the information on the hub to be created</param>
        /// <param name="moderator">The <see cref="ModeratorDto" /> containing the owner's data</param>
        /// <returns>The Hub specific link</returns>
        public static string GenerateLink(HubCreationDto hub, ModeratorDto moderator)
        {
            var toHash = Encoding.UTF8.GetBytes(hub.Name + moderator.Id + moderator.Nickname);

            using var sha256Managed = new SHA256Managed();

            return WebEncoders.Base64UrlEncode(
                sha256Managed.ComputeHash(toHash));
        }
    }
}
