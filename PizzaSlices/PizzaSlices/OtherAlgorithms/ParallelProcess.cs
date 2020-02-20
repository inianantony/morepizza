using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaSlices.OtherAlgorithms
{
    public class ParallelProcess
    {
        private object _obj = new object();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public int[] _previousChosenCombination = new List<int>().ToArray();
        public ulong _previousChosenSum = 0ul;
        private int _index = 0;

        public SliceAnswer Find(int[] source, ulong totalPieces)
        {
            var middlePoint = (source.Length - 6) / 2 + 6;
            ParallelOptions po = new ParallelOptions { CancellationToken = _cts.Token };
            
            var t1 = Task.Run(() => { FindFirstHalf(source, totalPieces, middlePoint, po); });
            var t2 = Task.Run(() => { FindSecondHalf(source, totalPieces, middlePoint, po); });
            FindFirst6(source, totalPieces, po);

            Task.WaitAll(t1, t2);

            return new SliceAnswer
            {
                PizzaTypes = _previousChosenCombination.Length,
                Combination = _previousChosenCombination,
            };
        }

        private void FindFirst6(int[] source, ulong totalPieces, ParallelOptions po)
        {
            try
            {
                var maxLength = source.Length < 7 ? source.Length : 7;
                for (int combinationFor = 1; combinationFor < maxLength; combinationFor++)
                {
                    if (GetPreviousChosenSum() == totalPieces)
                    {
                        _cts.Cancel();
                        break;
                    }

                    Action(source, totalPieces, combinationFor, $"{combinationFor} on MainTask", po.CancellationToken);
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Operation Completed");
            }
        }

        private void FindSecondHalf(int[] source, ulong totalPieces, int middlePoint, ParallelOptions po)
        {
            try
            {
                for (int combinationFor = source.Length + 1; combinationFor > middlePoint - 1; combinationFor -= 3)
                {
                    int start = combinationFor - 3 > middlePoint - 1 ? combinationFor - 3 : middlePoint - 1;
                    int end = combinationFor;
                    Parallel.For(start, end, po,
                        idx =>
                        {
                            if (idx < GetIndex()) return;
                            Action(source, totalPieces, idx, $"{idx} on T2", po.CancellationToken);
                        });
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Operation Completed");
            }
        }

        private void FindFirstHalf(int[] source, ulong totalPieces, int middlePoint, ParallelOptions po)
        {
            try
            {
                for (int combinationFor = 7; combinationFor < middlePoint; combinationFor += 3)
                {
                    int start = combinationFor;
                    int end = combinationFor + 3 < middlePoint ? combinationFor + 3 : middlePoint - 1;

                    Parallel.For(start, end, po,
                        idx =>
                        {
                            if (idx < GetIndex()) return;
                            Action(source, totalPieces, idx, $"{idx} on T1", po.CancellationToken);
                        });
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Operation Completed");
            }
        }

        private void UpdateState(int[] combination, ulong sum, ulong totalPieces, int index)
        {
            lock (_obj)
            {
                if (sum == totalPieces || sum < totalPieces && sum > GetPreviousChosenSum())
                {
                    _previousChosenCombination = combination;
                    _previousChosenSum = sum;
                    _index = index;
                }
            }
            if (GetPreviousChosenSum() == totalPieces)
                _cts.Cancel();
        }

        private ulong GetPreviousChosenSum()
        {
            lock (_obj)
            {
                return _previousChosenSum;
            }
        }

        private int GetIndex()
        {
            lock (_obj)
            {
                return _index;
            }
        }

        private void Action(int[] array, ulong totalPieces, int index, string message, CancellationToken token)
        {
            Console.WriteLine($"Processing : {message}");
            foreach (var combination in Combinations.CombinationsRosettaWoRecursion(array, index, token))
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                var sum = combination.Aggregate(0UL, (a, c) => a + (ulong)c);

                UpdateState(combination, sum, totalPieces, index);

            }
            Console.WriteLine($"Processed : {message}");
        }
    }

    static class Combinations
    {
        // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
        // in lexicographic order (first [0, 1, 2, ..., m-1]).
        private static IEnumerable<int[]> CombinationsRosettaWoRecursion(int m, int n, CancellationToken token)
        {
            int[] result = new int[m];
            Stack<int> stack = new Stack<int>(m);
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < n)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != m) continue;
                    yield return (int[])result.Clone();
                    break;
                }
            }
        }

        public static IEnumerable<T[]> CombinationsRosettaWoRecursion<T>(T[] array, int m, CancellationToken token)
        {
            if (array.Length < m)
                throw new ArgumentException("Array length can't be less than number of selected elements");
            if (m < 1)
                throw new ArgumentException("Number of selected elements can't be less than 1");
            T[] result = new T[m];
            foreach (int[] j in CombinationsRosettaWoRecursion(m, array.Length, token))
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                for (int i = 0; i < m; i++)
                {
                    result[i] = array[j[i]];
                }
                yield return result;
            }
        }
    }
}
