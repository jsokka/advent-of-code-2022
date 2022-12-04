namespace AdventOfCode2022.Puzzles;

internal class Day04 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day04.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var pairs = GetPairs(inputData);
        var overlapCount = 0;

        foreach (var pair in pairs)
        {
            var first = pair[0];
            var second = pair[1];
            if ((first.Item1 <= second.Item1 && first.Item2 >= second.Item2)
                || (second.Item1 <= first.Item1 && second.Item2 >= first.Item2))
            {
                overlapCount++;
            }
        }

        return overlapCount.ToString();
    }

    private static string Part2(string[] inputData)
    {
        var pairs = GetPairs(inputData);
        var overlapCount = 0;

        foreach (var pair in pairs)
        {
            var first = pair[0];
            var second = pair[1];
            if ((first.Item1 >= second.Item1 && first.Item1 <= second.Item2)
                || (second.Item1 >= first.Item1 && second.Item1 <= first.Item2))
            {
                overlapCount++;
            }
        }

        return overlapCount.ToString();
    }

    private static List<(int, int)[]> GetPairs(string[] inputData)
    {
        var pairs = new List<(int, int)[]>();

        for (int i = 0; i < inputData.Length; i++)
        {
            var line = inputData[i].Split(',');
            var pair = line.Select(p =>
            {
                var range = p.Split('-');
                return (Convert.ToInt32(range[0]), Convert.ToInt32(range[1]));
            }).ToArray();

            pairs.Add(pair);
        }

        return pairs;
    }
}
