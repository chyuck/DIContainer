using DIContainer.Attributes;

namespace DIContainer.Tests.TestTypes
{
    internal class ContainerTestClassWithInjectedInstances : IContainerTestInterfaceWithInjectedInstances
    {
        private readonly IContainerTestInterface _constructorInjectedInstance;

        [Inject]
        public ContainerTestClassWithInjectedInstances(IContainerTestInterface constructorInjectedInstance)
        {
            _constructorInjectedInstance = constructorInjectedInstance;
        }

        [Inject]
        public IContainerTestInterface2 PropertyInjectedInstance { get; set; }

        public IContainerTestInterface ConstructorInjectedInstance
        {
            get { return _constructorInjectedInstance; }
        }
    }
}
