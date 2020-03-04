namespace InTechNet.Common.Utils.Configuration.Helper
{
    /// <summary>
    /// Provides a helper for InTechNet metadata
    /// </summary>
    public class ProjectDto
    {
        /// <inheritdoc cref="ContactDto"/>
        public ContactDto Contact { get; set; }
        
        /// <summary>
        /// InTechNet API description
        /// </summary>
        public string Description { get; set; }

        /// <inheritdoc cref="LicenseDto"/>
        public LicenseDto License { get; set; }
        
        public string TermsOfService { get; set; }
        
        /// <summary>
        /// InTechNet API title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// InTechNet API version
        /// </summary>
        public string Version { get; set; }
    }
}
