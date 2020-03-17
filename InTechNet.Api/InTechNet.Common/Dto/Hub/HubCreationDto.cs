namespace InTechNet.Common.Dto.Hub
{
    /// <summary>
    /// <see cref="Hub" /> specialized for hub creation
    /// </summary>
    public class HubCreationDto
    {
        /// <summary>
        /// Hub name, must be unique
        /// </summary>
        public string Name { get; set; }
    }
}
