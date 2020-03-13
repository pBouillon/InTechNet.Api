namespace InTechNet.Service.Authentication.Interfaces
{
    /// <summary>
    /// Authentication service providing helpers and utils for the authentication process
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Generate a valid JWT for the moderator
        /// </summary>
        /// <returns>The valid JWT for the moderator</returns>
        string GetModeratorToken();

        /// <summary>
        /// Generate a valid JWT for the pupil
        /// </summary>
        /// <returns>The valid JWT for the pupil</returns>
        public string GetPupilToken();
    }
}
