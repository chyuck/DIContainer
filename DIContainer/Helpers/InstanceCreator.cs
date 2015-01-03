using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DIContainer.Entities;
using DIContainer.Exceptions;

namespace DIContainer.Helpers
{
    internal static class InstanceCreator
    {
        public static Instance Create(IReadOnlyContainer container, Type type, IDictionary<Type, object> unknownTypes)
        {
            Checker.ArgumentIsNull(container, "container");
            Checker.ArgumentIsNull(type, "type");

            var constructorsWithInjectAttr = InjectAttributeHelper.GetConstructors(type);

            if (constructorsWithInjectAttr.Length > 1)
                throw new ContainerException(
                    "Ambiguous usage of 'Inject' attribute on constructors of type '{0}'. Only one constructor can be marked with 'Inject' attribute.",
                    type);

            if (constructorsWithInjectAttr.Length == 0)
                return CreateInstanceUsingDefaultConstructor(type);

            return CreateInstanceUsingCustomConstructor(container, constructorsWithInjectAttr[0], unknownTypes);
        }

        private static Instance CreateInstanceUsingDefaultConstructor(Type type)
        {
            Checker.ArgumentIsNull(type, "type");

            var defaultConstructor =
                    type.GetConstructors(ConstantHelper.BINDING_FLAGS)
                        .SingleOrDefault(c => !c.GetParameters().Any());

            if (defaultConstructor == null)
                throw new ContainerException(
                    "Type '{0}' does not have either default constructor or constructor marked with 'Inject' attribute.",
                    type);

            var obj = Activator.CreateInstance(type, ConstantHelper.BINDING_FLAGS, null, null, null);

            return new Instance(obj);
        }

        private static Instance CreateInstanceUsingCustomConstructor(IReadOnlyContainer container, ConstructorInfo constructorInfo, IEnumerable<KeyValuePair<Type, object>> unknownTypes)
        {
            Checker.ArgumentIsNull(container, "container");
            Checker.ArgumentIsNull(constructorInfo, "constructorInfo");

            var type = constructorInfo.DeclaringType;

            var typesOfParameters =
                constructorInfo.GetParameters()
                    .Select(p => p.ParameterType)
                    .ToArray();

            var unregisteredParameterTypes =
                typesOfParameters
                    .Where(t => !container.Contains(t))
                    .ToArray();

            var missedParameterTypes =
                unregisteredParameterTypes
                    .Where(t => !unknownTypes.Any(kv => t.IsAssignableFrom(kv.Key)))
                    .ToArray();

            if (missedParameterTypes.Any())
            {
                var stringOfTypes = string.Join(",", missedParameterTypes.Select(t => t.ToString()));
                throw new ContainerException(
                    "The types ({0}) which are the parameters of constructor of type '{1}' are not registered in the container or not passed as unknown instances.",
                    stringOfTypes, type);
            }

            var instancesOfParameters =
                typesOfParameters
                    .Select(t =>
                    {
                        if (container.Contains(t))
                            return container.Get(t);

                        return unknownTypes.First(kv => t.IsAssignableFrom(kv.Key)).Value;
                    })
                    .ToArray();

            var obj = Activator.CreateInstance(type, ConstantHelper.BINDING_FLAGS, null, instancesOfParameters, null);

            return new Instance(obj);
        }
    }
}
