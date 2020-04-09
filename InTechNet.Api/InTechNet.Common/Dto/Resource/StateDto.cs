namespace InTechNet.Common.Dto.Resource
{
    /// <summary>
    /// Representation of a resource's state
    /// </summary>
    public class StateDto
    {
        /// <summary>
        /// Unique ID of the state
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The resource the pupil is in
        /// </summary>
        public ResourceDto Resource { get; set; }
    }
}
