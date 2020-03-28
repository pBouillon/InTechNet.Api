namespace InTechNet.Exception.Attendee
{
    /// <summary>
    /// Exception to be thrown when attempting to add an already registered attendee on this hub
    /// </summary>
    public class AttendeeAlreadyRegisteredException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Attendee already registered on this hub";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public AttendeeAlreadyRegisteredException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
