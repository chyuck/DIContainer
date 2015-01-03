using System;

namespace DIContainer.Helpers
{
    internal static class Checker
    {
        public static void ArgumentIsNull<TArgument>(TArgument arg, string argumentName)
            where TArgument : class
        {
            if (arg == null)
            {
                var message = string.Format("Argument '{0}' cannot be null.", argumentName);
                throw new ArgumentNullException(argumentName, message);
            }
        }

        public static void ArgumentIsEmpty(string arg, string argumentName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                var message = string.Format("Argument '{0}' cannot be null or empty string.", argumentName);
                throw new ArgumentException(message, argumentName);
            }
        }
    }
}
