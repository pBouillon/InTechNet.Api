namespace InTechNet.Service.User.Models
{
    public class ModeratorDto
    {
        public int IdModerator { get; set; }

        ///<summary>
        /// Nickname of the moderator
        ///</summary>
        public string ModeratorNickname { get; set; }

        ///<summary>
        /// Email of the moderator
        ///</summary>
        public string ModeratorEmail { get; set; }

        public string StringifiedId
            => IdModerator.ToString();
    }
}
