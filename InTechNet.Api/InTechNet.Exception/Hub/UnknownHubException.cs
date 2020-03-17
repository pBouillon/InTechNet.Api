namespace InTechNet.Exception.Hub
{
    /// <summary>
    /// Exception to be thrown when attempting to query an unknown hub
    /// in general or in a given context
    /// </summary>
    public class UnknownHubException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unable to find this hub in the given context";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public UnknownHubException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
