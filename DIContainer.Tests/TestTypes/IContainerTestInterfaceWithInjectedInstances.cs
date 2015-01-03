namespace DIContainer.Tests.TestTypes
{
    internal interface IContainerTestInterfaceWithInjectedInstances
    {
        IContainerTestInterface2 PropertyInjectedInstance { get; }

        IContainerTestInterface ConstructorInjectedInstance { get; }
    }
}
