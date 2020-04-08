namespace InTechNet.Exception.Module
{
    /// <summary>
    /// Exception to be thrown when attempting to perform an operation on a module
    /// that is not allowed in the current context
    /// </summary>
    public class IllegalModuleOperationException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "The requested operation can not be performed in the current context";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public IllegalModuleOperationException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
