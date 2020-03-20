using System.Net;

namespace InTechNet.Api.Errors.Classes
{
    /// <summary>
    /// Represent a custom HTTP Error response for error 409
    /// </summary>
    public class ConflictError : BaseApiError
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ex">Exception raised on this code</param>
        public ConflictError(System.Exception ex)
            : base(HttpStatusCode.Conflict, ex) { }
    }
}