using System.Collections.Generic;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace InTechNet.DataAccessLayer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.Email()
            };
        }

        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("apiModerator", "InTechNetModeratorAPI")
            };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "InTechNetModeratorAPI",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("secretModerator".Sha256())
                    },
                    AllowedScopes = {"apiModerator", StandardScopes.OfflineAccess}
                }
            };
        }
    }
}
