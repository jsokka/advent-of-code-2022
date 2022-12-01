namespace AdventOfCode2022.Puzzles;

internal class Day01 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<int?>("Day01.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(int?[] inputData)
    {
        return GetCaloriesByElves(inputData).Max().ToString();
    }

    private static string Part2(int?[] inputData)
    {
        return GetCaloriesByElves(inputData).OrderDescending().Take(3).Sum().ToString();
    }

    private static IEnumerable<int> GetCaloriesByElves(int?[] inputData)
    {
        var currentElf = 0;

        for (int i = 0; i < inputData.Length; i++)
        {
            var calories = inputData[i];
            if (!calories.HasValue || i == inputData.Length - 1)
            {
                yield return currentElf;
                currentElf = 0;
                continue;
            }

            currentElf += calories.Value;
        }
    }
}
