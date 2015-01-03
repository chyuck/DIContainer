namespace DIContainer.Tests.TestTypes
{
    internal interface IContainerTestInterfaceWithUnknownInstance
    {
        ContainerUnknownInstanceTestClass PropertyUnknownInstance { get; }

        ContainerUnknownInstanceTestClass ConstructorUnknownInstance { get; }
    }
}
