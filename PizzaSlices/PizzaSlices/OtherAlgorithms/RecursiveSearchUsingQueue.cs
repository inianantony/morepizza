using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PizzaSlices.OtherAlgorithms
{
    public class RecursiveSearchUsingQueue
    {
        private readonly ConcurrentDictionary<string, int> _nextInputs = new ConcurrentDictionary<string, int>();

        public List<int> GetCombination(int combinationCount, List<int> source)
        {
            _nextInputs[string.Join(" ", source.ToArray())] = source.Sum();
            var previousChosenCombination = new List<int>();
            int previousChosenSum = 0;
            while (_nextInputs.Count > 0)
            {
                var kvp = _nextInputs.First();
                _nextInputs.TryRemove(kvp.Key, out var sum);

                var input = kvp.Key.Split(' ').Select(int.Parse).ToList();

                if (sum == combinationCount)
                    return input;

                if (sum < combinationCount && sum > previousChosenSum)
                {
                    previousChosenCombination = input;
                    previousChosenSum = input.Sum();
                }

                var binary = Enumerable.Repeat<short>(0, input.Count).ToList();

                for (int i = 0; i < binary.Count && binary.Count > 1; i++)
                {
                    if (i > 0) binary[i - 1] = 0;
                    binary[i] = 1;
                    var activeDigits = GetActiveDigits(input, binary);
                    var newInput = string.Join(" ", activeDigits.ToArray());
                    if (!_nextInputs.ContainsKey(newInput))
                    {
                        _nextInputs[newInput] = activeDigits.Sum();
                    }
                }
            }

            return previousChosenCombination;
        }

        private static List<int> GetActiveDigits(List<int> input, List<short> binary)
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
