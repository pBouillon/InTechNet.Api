namespace InTechNet.Exception.Module
{
    /// <summary>
    /// Exception to be thrown when attempting to query an unknown state
    /// </summary>
    public class UnknownStateException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unable to find this state in the given context";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public UnknownStateException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
