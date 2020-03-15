using InTechNet.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace InTechNet.Api.Attributes
{
    /// <summary>
    /// Define an attribute to block non-pupil users from an endpoint
    /// <para>
    /// To pass, the user must pass the <see cref="PupilClaimRequiredFilter" />
    /// </para>
    /// </summary>
    public class PupilClaimRequiredAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Default endpoint
        /// </summary>
        public PupilClaimRequiredAttribute()
            : base(typeof(PupilClaimRequiredFilter)) { }
    }
}
