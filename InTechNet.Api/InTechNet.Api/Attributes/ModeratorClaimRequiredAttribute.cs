using InTechNet.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace InTechNet.Api.Attributes
{
    public class ModeratorClaimRequiredAttribute : TypeFilterAttribute
    {
        public ModeratorClaimRequiredAttribute() 
            : base(typeof(ModeratorClaimRequiredFilter)) { }
    }
}
