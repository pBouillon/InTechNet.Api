namespace InTechNet.Exception.Hub
{
    /// <summary>
    /// Exception to be thrown when attempting to use a hub
    /// name that is already in use for another hub of the
    /// same moderator
    /// </summary>
    public class DuplicatedHubNameException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "This hub name is already in use for the current moderator";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public DuplicatedHubNameException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
