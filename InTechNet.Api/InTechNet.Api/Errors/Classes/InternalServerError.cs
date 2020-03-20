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
        /// <param name="ex">Exception raised on this code</param>
        public InternalServerError(System.Exception ex)
            : base(HttpStatusCode.InternalServerError, ex) { }
    }
}
