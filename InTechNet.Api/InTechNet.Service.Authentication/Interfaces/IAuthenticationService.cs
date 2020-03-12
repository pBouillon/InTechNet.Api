namespace InTechNet.Service.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Generate a valid JWT
        /// </summary>
        /// <returns></returns>
        string GetToken();
    }
}
