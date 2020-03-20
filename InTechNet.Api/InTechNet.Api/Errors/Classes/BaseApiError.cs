using InTechNet.Api.Errors.Interfaces;
using System.Net;

namespace InTechNet.Api.Errors.Classes
{
    /// <inheritdoc cref="IApiError"/>
    public abstract class BaseApiError : IApiError
    {
        /// <inheritdoc cref="IApiError.StatusCode"/>
        public int StatusCode { get; }

        /// <inheritdoc cref="IApiError.StatusDescription"/>
        public string StatusDescription { get; }

        /// <inheritdoc cref="IApiError.ErrorMessage"/>
        public string ErrorMessage { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="statusCode">The associated <see cref="HttpStatusCode"/></param>
        /// <param name="ex">Exception raised on this code</param>
        protected BaseApiError(HttpStatusCode statusCode, System.Exception ex)
            => (StatusCode, StatusDescription, ErrorMessage) = ((int) statusCode, statusCode.ToString(), ex.Message);
    }
}
