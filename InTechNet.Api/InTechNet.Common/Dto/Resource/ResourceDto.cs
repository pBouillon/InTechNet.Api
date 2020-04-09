namespace InTechNet.Common.Dto.Resource
{
    /// <summary>
    /// Represent a resource
    /// </summary>
    public class ResourceDto
    {
        /// <summary>
        /// Unique ID of the selected module
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Content of this resource
        /// </summary>
        public string Content { get; set; }
    }
}
