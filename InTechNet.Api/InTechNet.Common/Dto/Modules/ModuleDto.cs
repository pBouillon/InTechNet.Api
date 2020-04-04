using System.Collections.Generic;
using InTechNet.Common.Dto.Topic;

namespace InTechNet.Common.Dto.Modules
{
    /// <summary>
    /// Represents the <see cref="Module"/>
    /// </summary>
    public class ModuleDto
    {
        /// <summary>
        /// Module ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Module name
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// <see cref="ModuleTypeDto"/> of the current module
        /// </summary>
        public ModuleTypeDto ModuleType { get; set; }

        /// <summary>
        /// Collections of the current module <see cref="TagDto"/>
        /// </summary>
        public IEnumerable<TagDto> Tags { get; set; }

        /// <summary>
        /// Whether this module is active or not for the current hub
        /// </summary>
        public bool IsActive { get; set; }
    }
}
