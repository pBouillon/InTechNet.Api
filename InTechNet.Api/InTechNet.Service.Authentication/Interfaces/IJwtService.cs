namespace InTechNet.Service.Authentication.Interfaces
{
    /// <summary>
    /// Contract for JWT usage and generation
    /// </summary>
    public interface IJwtService
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
        string GetPupilToken();
    }
}
