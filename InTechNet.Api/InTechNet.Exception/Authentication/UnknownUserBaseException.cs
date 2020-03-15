namespace InTechNet.Exception.Authentication
{
    /// <summary>
    /// Exception to be thrown when failing to fetch a user from a predicate
    /// </summary>
    public class UnknownUserException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unknown user";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public UnknownUserException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
