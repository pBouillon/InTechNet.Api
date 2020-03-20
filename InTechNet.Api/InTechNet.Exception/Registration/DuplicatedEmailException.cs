namespace InTechNet.Exception.Registration
{
    /// <summary>
    /// Exception to be thrown when attempting to add a resource with an email
    /// violating the `unique` constraint
    /// </summary>
    public class DuplicatedEmailException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "An existing object matching this email is already recorded";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public DuplicatedEmailException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}