using System;

namespace DIContainer.Exceptions
{
    /// <summary>DI container exception</summary>
    [Serializable]
    public class ContainerException : Exception
    {
        public ContainerException(string message)
            : base(message)
        {
        }

        public ContainerException(string message, params object[] args)
            : this(string.Format(message, args))
        {
        }

        public ContainerException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        public ContainerException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Format(message, args))
        {
        }
    }
}
