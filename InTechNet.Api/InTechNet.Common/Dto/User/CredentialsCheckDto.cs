using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Dto.User
{
    /// <summary>
    /// DTO for credentials checks
    /// </summary>
    public class CredentialsCheckDto
    {
        /// <summary>
        /// Boolean property to be set to true if any identifier already exists in the given context
        /// </summary>
        public bool AreUnique { get; set; }

        /// <summary>
        /// User provided email address to be checked
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User provided nickname to be checked
        /// </summary>
        public string Nickname { get; set; }
    }
}
