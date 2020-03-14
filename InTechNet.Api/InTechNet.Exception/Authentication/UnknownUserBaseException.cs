namespace InTechNet.Exception.Authentication
{
    public class UnknownUserException : BaseException
    {
        protected const string ExceptionMessage = "Unknown user";

        public UnknownUserException(System.Exception innerException = null) 
            : base(ExceptionMessage, innerException) { }
    }
}
