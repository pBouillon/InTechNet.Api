using InTechNet.Common.Utils.Authentication.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace InTechNet.Api.Filters
{
    public class ModeratorClaimRequiredFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims
                .Any(_ => _.Type == ClaimTypes.Role
                          && _.Value == InTechNetRoles.Moderator);

            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
