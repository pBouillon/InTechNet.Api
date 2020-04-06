using InTechNet.Common.Dto.Topic;
using System.Collections.Generic;

namespace InTechNet.Common.Dto.Modules
{
    /// <summary>
    /// Represents the <see cref="Module"/>
    /// </summary>
    public class PupilModuleDto
    {
        /// <summary>
        /// Module ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Module name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Module description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Collections of the current module <see cref="TagDto"/>
        /// </summary>
        public IEnumerable<TagDto> Tags { get; set; }

        /// <summary>
        /// Whether this module was started by the pupil
        /// </summary>
        public bool IsOnGoing { get; set; }
    }
}
