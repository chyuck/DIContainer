using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DIContainer.Exceptions;

namespace DIContainer.Helpers
{
    internal static class PropertyInjector
    {
        public static void Inject(IReadOnlyContainer container, object instance, IDictionary<Type, object> unknownTypes)
        {
            Checker.ArgumentIsNull(container, "container");
            Checker.ArgumentIsNull(instance, "instance");

            var type = instance.GetType();

            var propertiesWithInjectAttr = InjectAttributeHelper.GetProperties(type);

            CheckTypesOfPropertiesAreRegistered(container, type, propertiesWithInjectAttr, unknownTypes);

            foreach (var propertyInfo in propertiesWithInjectAttr)
            {
                var propertyType = propertyInfo.PropertyType;

                var instanceOfPropertyType =
                    container.Contains(propertyType)
                        ? container.Get(propertyType)
                        : unknownTypes.First(kv => propertyType.IsAssignableFrom(kv.Key)).Value;

                propertyInfo.SetValue(instance, instanceOfPropertyType, null);
            }
        }

        private static void CheckTypesOfPropertiesAreRegistered(IReadOnlyContainer container, Type type, PropertyInfo[] properties, IEnumerable<KeyValuePair<Type, object>> unknownTypes)
        {
            Checker.ArgumentIsNull(container, "container");
            Checker.ArgumentIsNull(type, "type");
            Checker.ArgumentIsNull(properties, "properties");
            
            var typesOfProperties =
                properties
                    .Select(p => p.PropertyType)
                    .ToArray();

            var unregisteredPropertyTypes =
                typesOfProperties
                    .Where(t => !container.Contains(t))
                    .ToArray();

            var missedPropertyTypes =
                unregisteredPropertyTypes
                    .Where(t => !unknownTypes.Any(kv => t.IsAssignableFrom(kv.Key)))
                    .ToArray();

            if (missedPropertyTypes.Any())
            {
                var stringOfTypes = string.Join(",", missedPropertyTypes.Select(t => t.ToString()));
                throw new ContainerException(
                    "The property types ({0}) of type '{1}' are not registered in the container or not passed as unknown instances.",
                    stringOfTypes, type);
            }
        }
    }
}
