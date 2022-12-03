namespace AdventOfCode2022.Puzzles;

internal class Day03 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day03.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var splitted = inputData.Select(s =>
        {
            var half = s.Length / 2;
            return (new string(s.Take(half).ToArray()), new string(s.TakeLast(half).ToArray()));
        });

        var mixed = new string(splitted.SelectMany(s => s.Item1.Intersect(s.Item2)).ToArray());

        return mixed.Sum(c => ItemTypes.IndexOf(c) + 1).ToString();
    }

    private static string Part2(string[] inputData)
    {
        var groups = new List<IEnumerable<string>>();
        var badges = new List<char>();

        for (int i = 0; i < inputData.Length; i += 3)
        {
            groups.Add(inputData.Skip(i).Take(3));
        }

        foreach (var g in groups)
        {
            badges.Add(g.First().First(c => g.All(s => s.Contains(c))));
        }

        return badges.Sum(c => ItemTypes.IndexOf(c) + 1).ToString();
    }

    private const string ItemTypes = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
}
