using System;
using System.Linq;
using System.Reflection;
using DIContainer.Attributes;

namespace DIContainer.Helpers
{
    internal static class InjectAttributeHelper
    {
        public static PropertyInfo[] GetProperties(Type type)
        {
            Checker.ArgumentIsNull(type, "type");

            return
                type.GetProperties(ConstantHelper.BINDING_FLAGS)
                    .Where(p =>
                        p.CanWrite &&
                        p.GetCustomAttributes(typeof(InjectAttribute), true).Any())
                    .ToArray();
        }

        public static ConstructorInfo[] GetConstructors(Type type)
        {
            Checker.ArgumentIsNull(type, "type");

            return
                type.GetConstructors(ConstantHelper.BINDING_FLAGS)
                    .Where(c =>
                        c.GetCustomAttributes(typeof(InjectAttribute), true).Any())
                    .ToArray();
        }
    }
}
