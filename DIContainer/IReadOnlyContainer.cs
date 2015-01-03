using System;
using DIContainer.Attributes;
using DIContainer.Exceptions;

namespace DIContainer
{
    /// <summary>Readonly DI container</summary>
    public interface IReadOnlyContainer : IDisposable
    {
        /// <summary>Returns an instance of registered <see cref="TKey"/> type from DI container</summary>
        /// <exception cref="ContainerException">If <see cref="ContainerException"/> type is not registered</exception>
        /// <exception cref="ArgumentException">If <see cref="unknownInstances"/> has more than one instance of the same type</exception>
        TKey Get<TKey>(params object[] unknownInstances);

        /// <summary>Returns an instance of registered <see cref="keyType"/> type from DI container</summary>
        /// <exception cref="ArgumentNullException">If <see cref="keyType"/> is null</exception>
        /// <exception cref="ContainerException">If <see cref="keyType"/> type is not registered</exception> 
        /// <exception cref="ArgumentException">If <see cref="unknownInstances"/> has more than one instance of the same type</exception>
        object Get(Type keyType, params object[] unknownInstances);

        /// <summary>Checks whether <see cref="TKey"/> is registered in DI container</summary>
        bool Contains<TKey>();

        /// <summary>Checks whether <see cref="keyType"/> is registered in DI container</summary>
        /// <exception cref="ArgumentNullException">If <see cref="keyType"/> is null</exception>
        bool Contains(Type keyType);

        /// <summary>Returns all types registered in DI container</summary>
        Type[] All { get; }

        /// <summary>Checks whether DI container holds an instance of <see cref="TKey"/></summary>
        /// <exception cref="ContainerException">If <see cref="TKey"/> is not registered in DI container</exception>
        bool ContainsInstance<TKey>();

        /// <summary>Checks whether DI container holds an instance of <see cref="keyType"/></summary>
        /// <exception cref="ArgumentNullException">If <see cref="keyType"/> is null</exception>
        /// <exception cref="ContainerException">If <see cref="keyType"/> is not registered in DI container</exception>
        bool ContainsInstance(Type keyType);
        
        /// <summary>Returns all instances of registered types that DI container holds</summary>
        object[] AllInstances { get; }

        /// <summary>Create an instance of <see cref="T"/> type and injects registered types</summary>
        /// <exception cref="ContainerException">If there is ambiguous usage of <see cref="InjectAttribute"/> attribute on constructors</exception>
        /// <exception cref="ContainerException">If injected type is not registered in DI container and it is not in <see cref="unknownInstances"/></exception>
        /// <exception cref="ArgumentException">If <see cref="unknownInstances"/> has more than one instance of the same type</exception>
        T CreateInstance<T>(params object[] unknownInstances);

        /// <summary>Creates an instance of <see cref="type"/> type and injects types</summary>
        /// <exception cref="ArgumentNullException">If <see cref="type"/> is null</exception> 
        /// <exception cref="ContainerException">If there is ambiguous usage of <see cref="InjectAttribute"/> attribute on constructors</exception>
        /// <exception cref="ContainerException">If injected type is not registered in DI container and it is not in <see cref="unknownInstances"/></exception>
        /// <exception cref="ArgumentException">If <see cref="unknownInstances"/> has more than one instance of the same type</exception>
        object CreateInstance(Type type, params object[] unknownInstances);

        /// <summary>The root which is used for syncronization of threads</summary>
        object SyncRoot { get; }
    }
}
