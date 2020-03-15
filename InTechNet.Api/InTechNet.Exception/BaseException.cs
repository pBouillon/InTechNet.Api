using System;

namespace InTechNet.Exception
{
    /// <summary>
    /// InTechNet custom exception base
    /// </summary>
    [Serializable]
    public abstract class BaseException : System.Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="exceptionMessage">Exception message</param>
        /// <param name="innerException">Nullable inner-exception</param>
        protected BaseException(string exceptionMessage, System.Exception innerException = null)
            : base(exceptionMessage, innerException) { }
    }
}
