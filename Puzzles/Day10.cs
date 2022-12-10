using System.Text;

namespace AdventOfCode2022.Puzzles;

internal class Day10 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day10.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var positions = GetSpritePositions(inputData);

        return positions.Where((_, i) => ((i + 1) - 20) % 40 == 0).Sum(p => p.SignalStrength).ToString();
    }

    private static string Part2(string[] inputData)
    {
        var positions = GetSpritePositions(inputData);
        var sb = new StringBuilder();

        foreach (var row in positions.Chunk(40))
        {
            sb.AppendLine();
            var col = 0;

            for (int i = 0; i < row.Length; i++)
            {
                sb.Append(Math.Abs(row[i].X - col) < 2 ? "#" : ".");
                col++;
            }
        }

        return sb.ToString();
    }

    private static IEnumerable<Sprite> GetSpritePositions(string[] inputData)
    {
        var x = 1;
        var cycle = 0;
        var addStack = new Stack<int>();

        for (int i = 0; i < inputData.Length; i++)
        {
            cycle++;
            yield return new Sprite(x, cycle * x);

            if (addStack.TryPop(out int add))
            {
                x += add;
                continue;
            }

            var parts = inputData[i].Split(' ');
            if (parts.Length == 1)
            {
                continue;
            }

            addStack.Push(int.Parse(parts[1]));
            i--;
        }
    }

    private sealed record Sprite(int X, int SignalStrength);
}
