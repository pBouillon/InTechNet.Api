using System.Net;
using InTechNet.Api.Errors.Interfaces;

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
        /// <param name="message"><inheritdoc cref="IApiError.ErrorMessage"/></param>
        protected BaseApiError(HttpStatusCode statusCode, string message = "")
            => (StatusCode, StatusDescription, ErrorMessage) = ((int) statusCode, statusCode.ToString(), message);
    }
}
