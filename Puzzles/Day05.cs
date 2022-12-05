using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Puzzles;

internal class Day05 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetRawInputDataAsync("Day05.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var stacks = GetInitialStacks(inputData).ToList();
        foreach (var (count, from, to) in ReadInstructions(inputData))
        {
            MoveCratesOneByOne(stacks, count, from, to);
        }

        return new string(stacks.Select(s => s.Peek()).ToArray());
    }

    private static string Part2(string[] inputData)
    {
        var stacks = GetInitialStacks(inputData).ToList();
        foreach (var (count, from, to) in ReadInstructions(inputData))
        {
            MoveCrates(stacks, count, from, to);
        }

        return new string(stacks.Select(s => s.Peek()).ToArray());
    }

    private static IEnumerable<Stack<char>> GetInitialStacks(string[] inputData)
    {
        var stacksEnd = GetInstructionsStartRow(inputData) - 3;

        var col = 0;

        while (true)
        {
            var stack = new Stack<char>();

            for (int row = stacksEnd; row >= 0; row--)
            {
                if (inputData[row].Length < col + 3)
                {
                    break;
                }

                var crate = inputData[row].Substring(col + 1, 1)[0];

                if (char.IsWhiteSpace(crate))
                {
                    break;
                }

                stack.Push(crate);
            }

            if (stack.Count == 0)
            {
                break;
            }

            yield return stack;
            col += 4;
        }
    }

    private static int GetInstructionsStartRow(string[] inputData)
    {
        for (int i = 0; i < inputData.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(inputData[i]))
            {
                return i + 1;
            }
        }

        throw new UnreachableException();
    }

    private static IEnumerable<(int, int, int)> ReadInstructions(string[] inputData)
    {
        var insturctionsStart = GetInstructionsStartRow(inputData);

        for (int i = insturctionsStart; i < inputData.Length; i++)
        {
            var numbers = Regex.Split(inputData[i], @"\D+")
                .Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            yield return (int.Parse(numbers[0]), int.Parse(numbers[1]), int.Parse(numbers[2]));
        }
    }

    private static void MoveCratesOneByOne(IEnumerable<Stack<char>> stacks, int count, int from, int to)
    {
        for (int i = 0; i < count; i++)
        {
            var crate = stacks.ElementAt(from - 1).Pop();
            stacks.ElementAt(to - 1).Push(crate);
        }
    }

    private static void MoveCrates(IEnumerable<Stack<char>> stacks, int count, int from, int to)
    {
        var tempStack = new Stack<char>();

        for (int i = 0; i < count; i++)
        {
            tempStack.Push(stacks.ElementAt(from - 1).Pop());
        }

        foreach (var crate in tempStack)
        {
            stacks.ElementAt(to - 1).Push(crate);
        }
    }
}
