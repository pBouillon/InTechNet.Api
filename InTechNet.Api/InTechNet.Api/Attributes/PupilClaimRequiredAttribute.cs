using InTechNet.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace InTechNet.Api.Attributes
{
    public class PupilClaimRequiredAttribute : TypeFilterAttribute
    {
        public PupilClaimRequiredAttribute()
            : base(typeof(PupilClaimRequiredFilter)) { }
    }
}
