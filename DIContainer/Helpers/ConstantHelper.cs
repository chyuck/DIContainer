using System.Reflection;

namespace DIContainer.Helpers
{
    internal static class ConstantHelper
    {
        public const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
    }
}
