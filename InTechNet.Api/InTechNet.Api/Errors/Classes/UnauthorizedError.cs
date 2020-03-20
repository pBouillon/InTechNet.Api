using System.Net;

namespace InTechNet.Api.Errors.Classes
{
    /// <summary>
    /// Represent a custom HTTP Error response for error 401
    /// </summary>
    public class UnauthorizedError : BaseApiError
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="message">Custom error message</param>
        public UnauthorizedError(string message = "")
            : base(HttpStatusCode.Unauthorized, message) { }
    }
}
