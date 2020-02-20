using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PizzaSlices
{
    public class SliceFinder
    {
        private readonly int[] _source;
        private readonly ulong _totalPieces;

        private int[] _previousChosenCombination = new List<int>().ToArray();
        private int[] _previousChosenIndexes = new List<int>().ToArray();
        private ulong _previousChosenSum = 0;
        int _index = 0;

        public SliceFinder(int[] source, ulong totalPieces)
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
                var smallSum = combinationOfSmallestNumbers.Aggregate(0UL, (a, c) => a + (ulong)c);
                if (smallSum > _totalPieces) continue;

                foreach (var combination in CombinationGenerator.Generate(_source, combinationFor))
                {
                    var sum = combination.Item1.Aggregate(0UL, (a, c) => a + (ulong)c);
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
                Combination = _previousChosenCombination,
                Pizzas = _previousChosenIndexes
            };
        }

        private bool CanStop()
        {
            return _previousChosenSum == _totalPieces;
        }

        private void UpdateState(Tuple<int[], int[]> combination, ulong sum, int index)
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