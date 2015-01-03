using System;
using DIContainer.Exceptions;

namespace DIContainer
{
    /// <summary>DI container</summary>
    public interface IContainer : IReadOnlyContainer
    {
        /// <summary>Registers <see cref="TImplementation"/> type as implementation of <see cref="TKey"/></summary>
        /// <typeparam name="TKey">Registration key type</typeparam>
        /// <typeparam name="TImplementation">Implementation type of key type</typeparam>
        /// <param name="lifetime">Lifetime of object in DI container</param>
        /// <exception cref="ContainerException">If <see cref="TKey"/> type is already registered</exception>
        void RegisterImplementation<TKey, TImplementation>(Lifetime lifetime = Lifetime.PerCall)
            where TImplementation : class, TKey;

        /// <summary>Registers <see cref="implementationType"/> type as implementation of <see cref="keyType"/></summary>
        /// <param name="keyType">Registration key type</param>
        /// <param name="implementationType">Implementation type of key type</param>
        /// <param name="lifetime">Lifetime of object in DI container</param>
        /// <exception cref="ArgumentNullException">if <see cref="keyType"/> is null</exception>
        /// <exception cref="ArgumentNullException">if <see cref="implementationType"/> is null</exception>
        /// <exception cref="ContainerException">If <see cref="keyType"/> type is already registered</exception>
        void RegisterImplementation(Type keyType, Type implementationType, Lifetime lifetime = Lifetime.PerCall);

        /// <summary>Registers object of <see cref="TKey"/> type</summary>
        /// <typeparam name="TKey">Registration key type</typeparam>
        /// <param name="instance">Object of <see cref="TKey"/> type</param>
        /// <remarks>This method registers <see cref="TKey"/> type with <see cref="Lifetime.PerContainer"/> lifetime</remarks>
        /// <exception cref="ContainerException">If <see cref="TKey"/> type is already registered</exception>
        void RegisterInstance<TKey>(TKey instance)
            where TKey : class;

        /// <summary>Registers object of any type which is implementation of <see cref="keyType"/></summary>
        /// <param name="keyType">Registration key type</param>
        /// <param name="instance">Object of a type which is implementation of <see cref="keyType"/> type</param>
        /// <remarks>This method registers <see cref="instance"/> object as an implementation of <see cref="keyType"/> with <see cref="Lifetime.PerContainer"/> lifetime</remarks>
        /// <exception cref="ArgumentNullException">if <see cref="keyType"/> is null</exception>
        /// <exception cref="ArgumentNullException">if <see cref="instance"/> is null</exception>
        /// <exception cref="ContainerException">If type of <see cref="instance"/> is not derived from <see cref="keyType"/> type</exception>
        /// <exception cref="ContainerException">If <see cref="keyType"/> type is already registered</exception>
        void RegisterInstance(Type keyType, object instance);

        /// <summary>Removes registration of <see cref="TKey"/> type from DI container</summary>
        /// <typeparam name="TKey">Registration key type</typeparam>
        /// <exception cref="ContainerException">If <see cref="TKey"/> type is not registered in DI container</exception>
        void Remove<TKey>();

        /// <summary>Removes registration of <see cref="keyType"/> type from DI container</summary>
        /// <param name="keyType">Registration key type</param>
        /// <exception cref="ArgumentNullException">if <see cref="keyType"/> is null</exception>
        /// <exception cref="ContainerException">If <see cref="keyType"/> type is not registered in DI container</exception>
        void Remove(Type keyType);

        /// <summary>Removes all registrations from DI container</summary>
        /// <remarks>It does not remove registrations for <see cref="IReadOnlyContainer"/> and <see cref="IContainer"/></remarks>
        void ClearAll();

        /// <summary>Remove instance of <see cref="TKey"/> from DI container</summary>
        /// <exception cref="ContainerException">If <see cref="TKey"/> type is not registered in DI container</exception>
        /// <exception cref="ContainerException">If DI container doesn't contain an instance of <see cref="TKey"/> type</exception>
        void RemoveInstance<TKey>();

        /// <summary>Remove instance of <see cref="keyType"/> from DI container</summary>
        /// <exception cref="ArgumentNullException">if <see cref="keyType"/> is null</exception>
        /// <exception cref="ContainerException">If <see cref="keyType"/> type is not registered in DI container</exception>
        /// <exception cref="ContainerException">If DI container doesn't contain an instance of <see cref="keyType"/> type</exception>
        void RemoveInstance(Type keyType);

        /// <summary>Removes all instances that DI container holds</summary>
        void ClearAllInstances();
    }
}
