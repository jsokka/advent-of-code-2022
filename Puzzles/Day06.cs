using System.Diagnostics;

namespace AdventOfCode2022.Puzzles;

internal class Day06 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetRawInputDataAsync("Day06.txt")).ToArray();

        return (Part1(inputData[0]), Part2(inputData[0]));
    }

    private static string Part1(string inputData)
    {
        return (GetMarkerIndex(inputData, 4) + 1).ToString();
    }

    private static string Part2(string inputData)
    {
        return (GetMarkerIndex(inputData, 14) + 1).ToString();
    }

    private static int GetMarkerIndex(string inputData, int length)
    {
        for (int i = length - 1; i < inputData.Length; i++)
        {
            if (inputData.Substring(i - length + 1, length).Distinct().Count() == length)
            {
                return i;
            }
        }

        throw new UnreachableException();
    }
}
