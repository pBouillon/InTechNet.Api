namespace InTechNet.Common.Dto.Topic
{
    /// <summary>
    /// Represents the module's <see cref="Tag"/>
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// Unique tag identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tag name
        /// </summary>
        /// <remarks>The name is unique among other tags</remarks>
        public string Name { get; set; }
    }
}
