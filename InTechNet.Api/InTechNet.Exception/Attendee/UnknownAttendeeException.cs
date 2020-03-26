namespace InTechNet.Exception.Attendee
{
    /// <summary>
    /// Exception to be thrown when attempting to query an unknown attendee
    /// </summary>
    public class UnknownAttendeeException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unknown attendee";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public UnknownAttendeeException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
