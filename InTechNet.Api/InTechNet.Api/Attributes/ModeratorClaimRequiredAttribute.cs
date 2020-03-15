using InTechNet.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace InTechNet.Api.Attributes
{
    /// <summary>
    /// Define an attribute to block non-moderator users from an endpoint
    /// <para>
    /// To pass, the user must pass the <see cref="ModeratorClaimRequiredFilter" />
    /// </para>
    /// </summary>
    public class ModeratorClaimRequiredAttribute : TypeFilterAttribute
    {
        /// <summary>
        ///Default constructor
        /// </summary>
        public ModeratorClaimRequiredAttribute()
            : base(typeof(ModeratorClaimRequiredFilter)) { }
    }
}
