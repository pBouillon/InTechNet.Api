namespace InTechNet.Common.Dto.Hub
{
    /// <summary>
    /// <see cref="HubDto" /> specialized for hub update
    /// </summary>
    public class HubUpdateDto
    {
        /// <summary>
        /// Hub name, must be unique
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the hub
        /// </summary>
        public string Description { get; set; }
    }
}
