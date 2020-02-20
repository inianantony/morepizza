using ExpectedObjects;
using NUnit.Framework;

namespace PizzaSlices.UnitTests
{
    public class SliceFinderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ExampleTestCase()
        {
            //Arrange
            var sliceFinder = new SliceFinder(new[] { 2, 5, 6, 8 }, 17);
            
            //Act
            var result = sliceFinder.Find();

            //Assert
            Assert.AreEqual(3, result.PizzaTypes);
            new[] { 0, 2, 3 }.ToExpectedObject().ShouldMatch(result.Pizzas);
        }
    }
}