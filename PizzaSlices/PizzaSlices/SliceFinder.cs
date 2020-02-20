using System.Collections;

namespace PizzaSlices
{
    public class SliceFinder
    {
        private readonly int[] _source;
        private readonly int _totalPieces;

        public SliceFinder(int[] source, int totalPieces)
        {
            _source = source;
            _totalPieces = totalPieces;
        }

        public SliceAnswer Find()
        {
            int combinationFor = 1;
            foreach (var combination in CombinationGenerator.Generate(_source, combinationFor))
            {

            }
            return new SliceAnswer
            {
                PizzaTypes = 3,
                Pizzas = new[] { 0 }
            };
        }
    }
}