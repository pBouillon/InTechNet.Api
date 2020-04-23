namespace InTechNet.Exception.Hub
{
    /// <summary>
    /// Exception to be thrown when moderator as create as many hubs
    /// as it is allowed to
    /// </summary>
    public class HubMaxCountReachedException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "This moderator has already reached its maximum number of hubs";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public HubMaxCountReachedException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
