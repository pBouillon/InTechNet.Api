namespace InTechNet.Exception.Authentication
{
    /// <summary>
    /// Exception to be thrown on an unexpected role for an operation
    /// </summary>
    public class IllegalRoleException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Illegal role for this operation.";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public IllegalRoleException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}