namespace InTechNet.Exception.Hub
{
    /// <summary>
    /// Exception to be thrown when attempting to perform an
    /// forbidden operation on a hub
    /// </summary>
    public class IllegalHubOperationException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unable to perform this operation on the hub";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public IllegalHubOperationException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
