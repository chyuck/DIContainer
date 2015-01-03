using System;
using DIContainer.Helpers;

namespace DIContainer.Entities
{
    internal class Instance
    {
        public Instance(object obj)
        {
            Checker.ArgumentIsNull(obj, "obj");

            Type = obj.GetType();
            Object = obj;
        }

        public Type Type { get; private set; }

        public object Object { get; private set; }
    }
}
