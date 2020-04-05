namespace InTechNet.Exception.Module
{
    /// <summary>
    /// Exception to be thrown when attempting to query an unknown module
    /// in general or in a given context
    /// </summary>
    public class UnknownModuleException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unable to find this module in the given context";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public UnknownModuleException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}