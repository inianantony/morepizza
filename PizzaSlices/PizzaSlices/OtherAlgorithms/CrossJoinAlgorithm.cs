using System.Collections.Generic;
using System.Linq;

namespace PizzaSlices.OtherAlgorithms
{
    public class CrossJoinAlgorithm
    {
        public IEnumerable<List<int>> Find(List<int> source, int totalPieces)
        {
            var crossJoin = from a in source
                            from b in source
                            select new List<int> { a, b };
            crossJoin = crossJoin.GroupBy(a => string.Join(" ", a.OrderBy(b => b).ToArray()))
                .Select(c => new { k = c.Key, lst = c.ToList() }).Where(a => a.lst.Count != 1).Select(a => a.lst.First());
            IEnumerable<List<int>> result = null;
            var checkForCompletion = crossJoin.Where(a => a.Sum() == totalPieces).Select(a => a);
            if (checkForCompletion.Any())
            {
                result = checkForCompletion;
            }

            var computed = crossJoin.ToList();
            for (int i = 2; i < source.Count; i++)
            {
                crossJoin = from a in computed
                            from b in source
                            let ints = new List<int>(a) { b }
                            select ints;
                crossJoin = crossJoin.GroupBy(a => string.Join(" ", a.OrderBy(b => b).ToArray()))
                    .Select(c => new { k = c.Key, lst = c.ToList() }).Where(a => a.lst.Count != 1)
                    .Select(a => a.lst.First());

                computed = crossJoin.ToList();

                checkForCompletion = computed.AsParallel().Where(a => a.Sum() == totalPieces).Select(a => a);
                if (checkForCompletion.Any())
                {
                    result = checkForCompletion;
                }
            }

            if (result == null)
            {
                var requiredPieces = totalPieces;
                while (true)
                {
                    checkForCompletion = crossJoin.Where(a => a.Sum() == requiredPieces).Select(a => a);
                    if (checkForCompletion.Any())
                    {
                        result = checkForCompletion;
                        break;
                    }

                    requiredPieces--;
                }
            }

            return result;
        }
    }
}
