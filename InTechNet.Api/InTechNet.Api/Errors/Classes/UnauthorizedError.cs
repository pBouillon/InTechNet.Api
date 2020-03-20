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
        /// <param name="ex">Exception raised on this code</param>
        public UnauthorizedError(System.Exception ex)
            : base(HttpStatusCode.Unauthorized, ex) { }
    }
}
