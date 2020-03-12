namespace InTechNet.Service.Authentication.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Verify if the provided token is valid
        /// </summary>
        /// <param name="token">The token to validate</param>
        void EnsureTokenValidity(string token);

        /// <summary>
        /// Generate a valid JWT
        /// </summary>
        /// <returns></returns>
        string GetToken();
    }
}
