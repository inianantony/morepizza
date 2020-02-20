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
            var sliceFinder = new SliceFinder(new[] { 2, 5, 6, 8 }, 17);
            var result = sliceFinder.Find();
            int pizzaTypes = result.PizzaTypes;
            int[] pizzas = result.Pizzas;
            Assert.AreEqual(3, pizzaTypes);
            Assert.AreEqual(new int[] { 0, 2, 3 }, pizzas);
        }
    }
}