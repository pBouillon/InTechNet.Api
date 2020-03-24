using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Dto.User.Pupil
{
    /// <summary>
    /// Light version of the <see cref="PupilDto"/>
    /// </summary>
    public class LightweightPupilDto
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nickname of the moderator
        /// </summary>
        public string Nickname { get; set; }
    }
}
