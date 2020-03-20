using System.Net;

namespace InTechNet.Api.Errors.Classes
{
    /// <summary>
    /// Represent a custom HTTP Error response for error 400
    /// </summary>
    public class BadRequestError : BaseApiError
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="message">Custom error message</param>
        public BadRequestError(string message = "")
            : base(HttpStatusCode.BadRequest, message) { }
    }
}
