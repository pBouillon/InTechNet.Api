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
        /// <param name="ex">Exception raised on this code</param>
        public BadRequestError(System.Exception ex)
            : base(HttpStatusCode.BadRequest, ex) { }
    }
}
