using DIContainer.Attributes;

namespace DIContainer.Tests.TestTypes
{
    internal class ContainerTestClassWithUnknownInstance : IContainerTestInterfaceWithUnknownInstance
    {
        private readonly ContainerUnknownInstanceTestClass _constructorUnknownInstance;

        [Inject]
        public ContainerTestClassWithUnknownInstance(ContainerUnknownInstanceTestClass constructorUnknownInstance)
        {
            _constructorUnknownInstance = constructorUnknownInstance;
        }

        [Inject]
        public ContainerUnknownInstanceTestClass PropertyUnknownInstance { get; set; }

        public ContainerUnknownInstanceTestClass ConstructorUnknownInstance 
        {
            get { return _constructorUnknownInstance; }
        }
    }
}
