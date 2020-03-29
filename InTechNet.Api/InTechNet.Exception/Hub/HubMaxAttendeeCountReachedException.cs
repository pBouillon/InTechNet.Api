namespace InTechNet.Exception.Hub
{
    /// <summary>
    /// Exception to be thrown when a hub is at maximum capacity
    /// same moderator
    /// </summary>
    public class HubMaxAttendeeCountReachedException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "This hub already reached its maximum allowed number of attendees";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public HubMaxAttendeeCountReachedException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
