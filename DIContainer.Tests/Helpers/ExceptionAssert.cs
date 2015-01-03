using System;
using DIContainer.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DIContainer.Tests.Helpers
{
    internal static class ExceptionAssert
    {
        public static void Thrown<TException>(Action action, Action<TException> onExceptionThrown = null)
            where TException : Exception
        {
            Checker.ArgumentIsNull(action, "action");
            
            try
            {
                action();
                Assert.Fail("Exception of type '{0}' is expected.", typeof(TException));
            }
            catch (TException ex)
            {
                if (onExceptionThrown != null)
                    onExceptionThrown(ex);
            }
        }

        public static void Thrown<TException>(Action action, string exceptionMessage)
            where TException : Exception
        {
            Thrown<TException>(action, ex => Assert.AreEqual(exceptionMessage, ex.Message));
        }
    }
}
