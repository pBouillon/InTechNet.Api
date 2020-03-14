using System;

namespace InTechNet.Exception
{
    [Serializable]
    public abstract class BaseException : System.Exception
    {
        protected BaseException(string exceptionMessage, System.Exception innerException = null)
            : base (exceptionMessage, innerException) { }
    }
}
