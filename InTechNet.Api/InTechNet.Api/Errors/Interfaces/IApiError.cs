using Newtonsoft.Json;

namespace InTechNet.Api.Errors.Interfaces
{
    /// <summary>
    /// Defines API error response contract
    /// </summary>
    public interface IApiError
    {
        /// <summary>
        /// Handled error HTTP status
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// HTTP status description
        /// </summary>
        public string StatusDescription { get; }

        /// <summary>
        /// Application specific error message
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ErrorMessage { get; }
    }
}
