using InTechNet.Common.Dto.Subscription;
using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Dto.User.Moderator
{
    /// <summary>
    /// <see cref="Moderator" /> DTO
    /// </summary>
    public class ModeratorDto
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nickname of the moderator
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Nickname { get; set; }

        /// <summary>
        /// Email of the moderator
        /// </summary>
        [Required]
        [MaxLength(128)] 
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Number of hub of the moderator
        /// </summary>
        [Range(0, double.MaxValue)]
        public int NumberOfHub { get; set; }

        /// <summary>
        /// Moderator's JWT
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Moderator's Subscription
        /// </summary>
        public SubscriptionPlanDto SubscriptionPlanDto { get; set; }
    }
}
