using System.Collections.Generic;
using System.Linq;

namespace PizzaSlices.OtherAlgorithms
{
    public class BinarySwappingAlgorithm
    {
        public List<int> GetCombination(List<int> source, int totalPieces)
        {
            if (source.Sum() <= totalPieces)
                return source;

            var binary = Enumerable.Repeat(0, source.Count).ToList();
            var expected = new List<int>(binary) { [0] = 1 };

            var previousChosenCombination = new List<int>();
            var previousChosenSum = 0;
            while (string.Join("", binary.ToArray()) != string.Join("", expected.ToArray()))
            {
                if (binary.Sum() % 2 == 0)
                {
                    binary[^1] = 1 - binary[^1];
                }
                else
                {
                    var indexOfOne = binary.LastIndexOf(1);
                    binary[indexOfOne - 1] = 1 - binary[indexOfOne - 1];
                }

                var newInput = GetActiveDigits(source, binary);
                var sum = newInput.Sum();

                if (sum == totalPieces)
                    return newInput;

                if (sum < totalPieces && sum > previousChosenSum)
                {
                    previousChosenCombination = newInput;
                    previousChosenSum = sum;
                }

                var newInputStr = string.Join(" ", newInput.ToArray());
                if (newInputStr != "")
                {
                }
            }
            return previousChosenCombination;
        }

        private static List<int> GetActiveDigits(List<int> input, List<int> binary)
        {
            var returnList = new List<int>();
            for (int j = 0; j < binary.Count; j++)
            {
                if (binary[j] == 0) returnList.Add(input[j]);
            }

            return returnList;
        }
    }
}
