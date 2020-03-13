namespace InTechNet.Service.Authentication.Interfaces
{
    /// <summary>
    /// Authentication service providing helpers and utils for the authentication process
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Verify if the provided token is valid
        /// </summary>
        /// <param name="token">The token to validate</param>
        void EnsureTokenValidity(string token);

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
