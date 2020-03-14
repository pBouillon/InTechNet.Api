namespace InTechNet.Exception
{
    public abstract class ExceptionBase : System.Exception
    {
        protected ExceptionBase(string exceptionMessage, System.Exception innerException = null)
            : base (exceptionMessage, innerException) { }
    }
}
