using System;
using System.Collections.Generic;

namespace PizzaSlices
{
    public class CombinationGenerator
    {
        public static IEnumerable<Tuple<T[], int[]>> Generate<T>(T[] source, int combinationFor)
        {
            if (source.Length < combinationFor)
                throw new ArgumentException("Source cant be less than the combination requested for");
            if (combinationFor < 1)
                throw new ArgumentException("We cant generate 0 combinations");

            T[] result = new T[combinationFor];
            foreach (int[] j in GetCombination(combinationFor, source.Length))
            {
                for (int i = 0; i < combinationFor; i++)
                {
                    result[i] = source[j[i]];
                }
                yield return new Tuple<T[],int[]>(result,j);
            }
        }

        private static IEnumerable<int[]> GetCombination(int combinationFor, int sourceLength)
        {
            int[] result = new int[combinationFor];
            Stack<int> stack = new Stack<int>(combinationFor);
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < sourceLength)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != combinationFor) continue;
                    yield return (int[])result.Clone();
                    break;
                }
            }
        }
    }
}