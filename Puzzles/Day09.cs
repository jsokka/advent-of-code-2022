using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode2022.Puzzles;

internal class Day09 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var motions = (await InputDataReader.GetInputDataAsync<string>("Day09.txt"))
             .Select(s =>
             {
                 var parts = s.Split(' ');
                 return (parts[0], int.Parse(parts[1]));
             }).ToArray();

        return (Part1(motions), Part2(motions));
    }

    private static string Part1((string, int)[] motions)
    {
        var tailPositions = Move(motions, 2);

        return tailPositions.Count.ToString();
    }

    private static string Part2((string, int)[] motions)
    {
        var tailPositions = Move(motions, 10);

        return tailPositions.Count.ToString();
    }

    private static HashSet<Vector2> Move((string, int)[] motions, int ropeLength)
    {
        var knots = Enumerable.Range(0, ropeLength).Select(_ => new Vector2(0, 0)).ToArray();

        var tailPositions = new HashSet<Vector2> { knots.Last() };

        foreach (var (direction, steps) in motions)
        {
            for (int i = 0; i < steps; i++)
            {
                MoveHead(ref knots[0], direction);

                for (int j = 1; j < knots.Length; j++)
                {
                    var knot = knots[j];
                    var head = knots[j - 1];

                    var distance = (head - knot).LengthSquared();

                    if (distance > 2)
                    {
                        if (knot.X != head.X)
                        {
                            knot.X += (head.X > knot.X ? 1 : -1);
                        }

                        if (knot.Y != head.Y)
                        {
                            knot.Y += (head.Y > knot.Y ? 1 : -1);
                        }

                        knots[j] = knot;
                    }

                    if (j == knots.Length - 1)
                    {
                        tailPositions.Add(knot);
                    }
                }
            }
        }

        return tailPositions;
    }

    private static void MoveHead(ref Vector2 head, string direction)
    {
        switch (direction)
        {
            case "U":
                head.Y++;
                break;
            case "D":
                head.Y--;
                break;
            case "R":
                head.X++;
                break;
            case "L":
                head.X--;
                break;
            default:
                throw new UnreachableException();
        }
    }
}
