namespace InTechNet.Exception.Authentication
{
    public class InvalidCredentialsException : BaseException
    {
        protected const string ExceptionMessage = "Invalid credentials.";

        public InvalidCredentialsException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
