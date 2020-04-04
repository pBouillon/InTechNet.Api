namespace InTechNet.Common.Dto.Modules
{
    /// <summary>
    /// Represents the module's <see cref="ModuleType"/>
    /// </summary>
    public class ModuleTypeDto
    {
        /// <summary>
        /// Id of this module type
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Common name of this module type
        /// </summary>
        public string Type { get; set; }
    }
}
