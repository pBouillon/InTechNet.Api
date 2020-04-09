namespace InTechNet.Exception.Resource
{
    /// <summary>
    /// Exception to be thrown when attempting to query an unknown resource
    /// in general or in a given context
    /// </summary>
    public class UnknownResourceException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unable to find this resource in the given context";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public UnknownResourceException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
