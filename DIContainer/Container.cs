using System;
using System.Collections.Generic;
using System.Linq;
using DIContainer.Entities;
using DIContainer.Exceptions;
using DIContainer.Helpers;

namespace DIContainer
{
    internal class Container : IContainer
    {
        #region Fields

        private readonly object _syncRoot = new object();

        private readonly IDictionary<Type, Implementation> _implementations;
        private readonly IDictionary<Type, Instance> _instances;

        #endregion


        #region Constructors

        public Container()
        {
            _implementations = new Dictionary<Type, Implementation>();
            _instances = new Dictionary<Type, Instance>();

            RegisterContainer();
        }

        #endregion


        #region IReadOnlyContainer

        public TKey Get<TKey>(params object[] unknownInstances)
        {
            return (TKey)Get(typeof(TKey), unknownInstances);
        }

        public object Get(Type keyType, params object[] unknownInstances)
        {
            Checker.ArgumentIsNull(keyType, "keyType");

            if (unknownInstances.GroupBy(i => i.GetType()).Any(g => g.Count() > 1))
                throw new ArgumentException("unknownInstances has more than one entry of the same type.", "unknownInstances");

            var unknownTypes =
                unknownInstances
                    .ToDictionary(i => i.GetType(), i => i);

            lock (_syncRoot)
            {
                if (!Contains(keyType))
                    throw new ContainerException("Container does not contain '{0}' type as a key.", keyType);

                {
                    Instance instance;
                    if (_instances.TryGetValue(keyType, out instance))
                        return instance.Object;
                }

                {
                    var implementation = _implementations[keyType];
                    var instance = InstanceCreator.Create(this, implementation.Type, unknownTypes);
                    var obj = instance.Object;
                    PropertyInjector.Inject(this, instance.Object, unknownTypes);

                    if (implementation.Lifetime == Lifetime.PerContainer)
                        _instances.Add(keyType, instance);

                    return obj;
                }
            }
        }

        public bool Contains<TKey>()
        {
            return Contains(typeof(TKey));
        }

        public bool Contains(Type keyType)
        {
            Checker.ArgumentIsNull(keyType, "keyType");

            lock (_syncRoot)
            {
                return _implementations.ContainsKey(keyType);
            }
        }

        public Type[] All
        {
            get
            {
                lock (_syncRoot)
                {
                    return _implementations.Keys.ToArray();
                }
            }
        }

        public bool ContainsInstance<TKey>()
        {
            return ContainsInstance(typeof(TKey));
        }

        public bool ContainsInstance(Type keyType)
        {
            Checker.ArgumentIsNull(keyType, "keyType");

            lock (_syncRoot)
            {
                if (!Contains(keyType))
                    throw new ContainerException("Container does not contain '{0}' type as a key.", keyType);

                return _instances.ContainsKey(keyType);
            }
        }

        public object[] AllInstances
        {
            get
            {
                lock (_syncRoot)
                {
                    return 
                        _instances.Values
                            .Select(i => i.Object)
                            .Distinct()
                            .ToArray();
                }
            }
        }

        public T CreateInstance<T>(params object[] unknownInstances)
        {
            return (T)CreateInstance(typeof(T), unknownInstances);
        }

        public object CreateInstance(Type type, params object[] unknownInstances)
        {
            Checker.ArgumentIsNull(type, "type");

            if (unknownInstances.GroupBy(i => i.GetType()).Any(g => g.Count() > 1))
                throw new ArgumentException("unknownInstances has more than one entry of the same type.", "unknownInstances");

            var unknownTypes =
                unknownInstances
                    .ToDictionary(i => i.GetType(), i => i);

            lock (_syncRoot)
            {
                var instance = InstanceCreator.Create(this, type, unknownTypes);
                PropertyInjector.Inject(this, instance.Object, unknownTypes);

                return instance.Object;
            }
        }

        public object SyncRoot
        {
            get { return _syncRoot; }
        }

        #endregion


        #region IContainer

        public void RegisterImplementation<TKey, TImplementation>(Lifetime lifetime = Lifetime.PerCall)
            where TImplementation : class, TKey
        {
            RegisterImplementation(typeof(TKey), typeof(TImplementation), lifetime);
        }

        public void RegisterImplementation(Type keyType, Type implementationType, Lifetime lifetime = Lifetime.PerCall)
        {
            Checker.ArgumentIsNull(keyType, "keyType");
            Checker.ArgumentIsNull(implementationType, "implementationType");

            if (!keyType.IsAssignableFrom(implementationType))
                throw new ContainerException("Type '{0}' must be derived from type '{1}'.", implementationType, keyType);

            lock (_syncRoot)
            {
                if (Contains(keyType))
                    throw new ContainerException("Container already contains '{0}' type as a key.", keyType);

                var implementation = new Implementation(implementationType, lifetime);

                _implementations.Add(keyType, implementation);
            }
        }

        public void RegisterInstance<TKey>(TKey instance)
            where TKey : class
        {
            RegisterInstance(typeof(TKey), instance);
        }

        public void RegisterInstance(Type keyType, object instance)
        {
            Checker.ArgumentIsNull(keyType, "keyType");
            Checker.ArgumentIsNull(instance, "instance");

            if (!keyType.IsInstanceOfType(instance))
                throw new ContainerException("Type '{0}' must be derived from type '{1}'.", instance.GetType(), keyType);

            lock (_syncRoot)
            {
                if (Contains(keyType))
                    throw new ContainerException("Container already contains '{0}' type as a key.", keyType);

                _implementations.Add(keyType, new Implementation(instance.GetType(), Lifetime.PerContainer));
                _instances.Add(keyType, new Instance(instance));
            }
        }

        public void Remove<TKey>()
        {
            Remove(typeof(TKey));
        }

        public void Remove(Type keyType)
        {
            Checker.ArgumentIsNull(keyType, "keyType");

            if (keyType == typeof(IReadOnlyContainer) || keyType == typeof(IContainer))
                throw new ContainerException("The key type '{0}' cannot be removed from container.", keyType);

            lock (_syncRoot)
            {
                if (!Contains(keyType))
                    throw new ContainerException("Container does not contain '{0}' type as a key.", keyType);

                if (_instances.ContainsKey(keyType))
                    _instances.Remove(keyType);

                _implementations.Remove(keyType);
            }
        }

        public void ClearAll()
        {
            lock (_syncRoot)
            {
                _implementations.Clear();
                _instances.Clear();

                RegisterContainer();
            }
        }

        public void RemoveInstance<TKey>()
        {
            RemoveInstance(typeof(TKey));
        }

        public void RemoveInstance(Type keyType)
        {
            Checker.ArgumentIsNull(keyType, "keyType");

            if (keyType == typeof(IReadOnlyContainer) || keyType == typeof(IContainer))
                throw new ContainerException("The instance of key type '{0}' cannot be removed from container.", keyType);

            lock (_syncRoot)
            {
                if (!Contains(keyType))
                    throw new ContainerException("Container does not contain '{0}' type as a key.", keyType);

                if (!ContainsInstance(keyType))
                    throw new ContainerException("Container does not contain an instance of '{0}' type.", keyType);

                _instances.Remove(keyType);
            }
        }

        public void ClearAllInstances()
        {
            lock (_syncRoot)
            {
                _instances.Clear();

                _instances.Add(typeof(IReadOnlyContainer), new Instance(this));
                _instances.Add(typeof(IContainer), new Instance(this));
            }
        }

        #endregion


        #region IDisposable

        public void Dispose()
        {
            _implementations.Clear();
            _instances.Clear();
        }

        #endregion


        #region Private Methods

        private void RegisterContainer()
        {
            RegisterInstance<IReadOnlyContainer>(this);
            RegisterInstance<IContainer>(this);
        }

        #endregion
    }
}
