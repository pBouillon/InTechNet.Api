namespace InTechNet.Exception.Authentication
{
    /// <summary>
    /// Exception to be thrown on mismatching credentials
    /// </summary>
    public class InvalidCredentialsException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Invalid credentials.";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public InvalidCredentialsException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException)
        {
        }
    }
}
