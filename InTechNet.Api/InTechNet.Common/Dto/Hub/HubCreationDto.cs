namespace InTechNet.Common.Dto.Hub
{
    /// <summary>
    /// <see cref="HubDto" /> specialized for hub creation
    /// </summary>
    public class HubCreationDto
    {
        /// <summary>
        /// Hub name, must be unique
        /// </summary>
        public string Name { get; set; }
    }
}
