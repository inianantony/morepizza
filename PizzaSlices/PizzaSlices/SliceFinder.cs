using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PizzaSlices
{
    public class SliceFinder
    {
        private readonly int[] _source;
        private readonly int _totalPieces;

        private int[] _previousChosenCombination = new List<int>().ToArray();
        private int[] _previousChosenIndexes = new List<int>().ToArray();
        private int _previousChosenSum = 0;
        int _index = 0;

        public SliceFinder(int[] source, int totalPieces)
        {
            _source = source;
            _totalPieces = totalPieces;
        }

        public SliceAnswer Find()
        {
            //nCr
            int combinationFor = _source.Length;
            for (; combinationFor > 0; combinationFor--)
            {
                var combinationOfSmallestNumbers = _source.Take(combinationFor);
                var smallSum = combinationOfSmallestNumbers.Aggregate(0, (a, c) => a + c);
                if (smallSum > _totalPieces) continue;

                foreach (var combination in CombinationGenerator.Generate(_source, combinationFor))
                {
                    var sum = combination.Item1.Aggregate(0, (a, c) => a + c);
                    UpdateState(combination, sum, combinationFor);

                    if (CanStop())
                    {
                        break;
                    }
                }

                break;
            }

            return new SliceAnswer
            {
                PizzaTypes = _previousChosenCombination.Length,
                Pizzas = _previousChosenIndexes
            };
        }

        private bool CanStop()
        {
            return _previousChosenSum == _totalPieces;
        }

        private void UpdateState(Tuple<int[], int[]> combination, int sum, int index)
        {
            if (sum == _totalPieces || sum < _totalPieces && sum > _previousChosenSum)
            {
                _previousChosenCombination = combination.Item1;
                _previousChosenIndexes = combination.Item2;
                _previousChosenSum = sum;
                _index = index;
            }
        }
    }
}