namespace InTechNet.Exception.Hub
{
    /// <summary>
    /// Exception to be thrown when a hub is at maximum capacity
    /// same moderator
    /// </summary>
    public class MaxAttendeeCountReachedException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "This hub is already full";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public MaxAttendeeCountReachedException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
