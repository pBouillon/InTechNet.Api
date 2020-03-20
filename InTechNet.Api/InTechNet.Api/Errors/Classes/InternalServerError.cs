using System.Net;

namespace InTechNet.Api.Errors.Classes
{
    /// <summary>
    /// Represent a custom HTTP Error response for error 500
    /// </summary>
    public class InternalServerError : BaseApiError
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="message">Custom error message</param>
        public InternalServerError(string message = "") 
            : base(HttpStatusCode.InternalServerError, message) { }
    }
}
