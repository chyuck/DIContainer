using DIContainer.Attributes;

namespace DIContainer.Tests.TestTypes
{
    internal class ContainerTestClassWithAmbiguousConstructors
    {
        [Inject]
        public ContainerTestClassWithAmbiguousConstructors(string s)
        {
        }

        [Inject]
        public ContainerTestClassWithAmbiguousConstructors(int i)
        {
        }
    }
}
