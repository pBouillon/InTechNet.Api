using System.Linq;
using System.Security.Claims;
using InTechNet.Common.Utils.Authentication.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InTechNet.Api.Filters
{
    /// <summary>
    /// Provide a filter to block non-pupil users
    /// <remarks>
    /// To access a resource, the user must have a claim of type <see cref="ClaimTypes.Role" />
    /// with the value <see cref="InTechNetRoles.Pupil" />
    /// </remarks>
    /// </summary>
    public class PupilClaimRequiredFilter : IAuthorizationFilter
    {
        /// <inheritdoc cref="IAuthorizationFilter.OnAuthorization" />
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims
                .Any(_ => _.Type == ClaimTypes.Role
                          && _.Value == InTechNetRoles.Pupil);

            if (!hasClaim) context.Result = new ForbidResult();
        }
    }
}
