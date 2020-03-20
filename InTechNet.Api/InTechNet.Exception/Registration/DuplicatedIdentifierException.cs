namespace InTechNet.Exception.Registration
{
    /// <summary>
    /// Exception to be thrown when attempting to add a resource with an identifier
    /// violating the `unique` constraint
    /// </summary>
    public class DuplicatedIdentifierException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "An existing object matching those identifiers is already recorded";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public DuplicatedIdentifierException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}