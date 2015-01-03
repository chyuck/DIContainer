using System;
using DIContainer.Helpers;

namespace DIContainer.Entities
{
    internal class Implementation
    {
        public Implementation(Type type, Lifetime lifetime)
        {
            Checker.ArgumentIsNull(type, "type");

            Type = type;
            Lifetime = lifetime;
        }

        public Type Type { get; private set; }

        public Lifetime Lifetime { get; private set; }
    }
}
