using System.Linq;
using DIContainer.Helpers;
using DIContainer.Tests.TestTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DIContainer.Tests
{
    [TestClass]
    public class InjectAttributeHelperTests
    {
        [TestMethod]
        public void GetConstructors()
        {
            // Arrange
            var type = typeof(InjectAttributeHelperTestClass);

            // Act
            var constructors = InjectAttributeHelper.GetConstructors(type);

            // Assert
            Assert.AreEqual(4, constructors.Length);

            var expectedParameterTypes = 
                new[]
                {
                    typeof (int), 
                    typeof (double), 
                    typeof (decimal), 
                    typeof (string)
                };
            var actualParameterTypes = constructors.Select(c => c.GetParameters().Single().ParameterType).ToArray();
            
            CollectionAssert.AreEqual(expectedParameterTypes, actualParameterTypes);
        }

        [TestMethod]
        public void GetProperties()
        {
            // Arrange
            var type = typeof(InjectAttributeHelperTestClass);

            // Act
            var properties = InjectAttributeHelper.GetProperties(type);

            // Assert
            Assert.AreEqual(7, properties.Length);

            var expectedPropertyNames =
                new[]
                {
                    "PublicProperty",
                    "PublicPropertyWithInternalSettter",
                    "PublicPropertyWithProtectedSettter",
                    "PublicPropertyWithPrivateSettter",
                    "InternalProperty",
                    "ProtectedProperty",
                    "PrivateProperty"
                };
            var actualPropertyNames = properties.Select(p => p.Name).ToArray();

            CollectionAssert.AreEqual(expectedPropertyNames, actualPropertyNames);
        }
    }
}
