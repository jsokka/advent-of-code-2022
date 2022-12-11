using System.Buffers;

namespace AdventOfCode2022.Puzzles;

internal class Day11 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day11.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var monkeys = ReadMonkeys(inputData).ToList();

        for (int i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    var item = monkey.InspectNextItem(true);
                    var monkeyToThrow = item % monkey.DivisibleBy == 0
                        ? monkey.ThrowToMoneyIfDivisble
                        : monkey.ThrowToMonkeyIfNotDivisble;
                    monkeys[monkeyToThrow].Items.Enqueue(item);
                }
            }
        }

        var topMonkeys = monkeys.OrderByDescending(m => m.InspectionCount).Take(2).Select(m => m.InspectionCount).ToArray();
        return (topMonkeys[0] * topMonkeys[1]).ToString();
    }

    private static string Part2(string[] inputData)
    {
        var monkeys = ReadMonkeys(inputData).ToList();

        var worryLevelLimit = monkeys.Aggregate(1, (c, m) => c * m.DivisibleBy);

        for (int i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    var item = monkey.InspectNextItem(false) % worryLevelLimit;
                    var monkeyToThrow = item % monkey.DivisibleBy == 0
                        ? monkey.ThrowToMoneyIfDivisble
                        : monkey.ThrowToMonkeyIfNotDivisble;
                    monkeys[monkeyToThrow].Items.Enqueue(item);
                }
            }
        }

        var topMonkeys = monkeys.OrderByDescending(m => m.InspectionCount).Take(2).Select(m => m.InspectionCount).ToArray();
        return (topMonkeys[0] * topMonkeys[1]).ToString();
    }

    private static IEnumerable<Monkey> ReadMonkeys(string[] inputData)
    {
        Monkey monkey = null!;
        var monkeys = new List<Monkey>();

        foreach (var line in inputData)
        {
            if (line.StartsWith("Monkey"))
            {
                monkey = new();
                monkeys.Add(monkey);
            }
            else if (line.StartsWith("Starting items:"))
            {
                var items = line.Replace("Starting items:", "")
                    .Split(',').Select(item => long.Parse(item.Trim())).ToList();
                monkey.Items = new Queue<long>(items);
            }
            else if (line.StartsWith("Operation:"))
            {
                monkey.Operation = line.Replace("Operation: new =", "").Trim();
            }
            else if (line.StartsWith("Test:"))
            {
                monkey.DivisibleBy = int.Parse(line.Replace("Test: divisible by", ""));
            }
            else if (line.StartsWith("If true:"))
            {
                monkey.ThrowToMoneyIfDivisble = int.Parse(line.Replace("If true: throw to monkey", ""));
            }
            else if (line.StartsWith("If false:"))
            {
                monkey.ThrowToMonkeyIfNotDivisble = int.Parse(line.Replace("If false: throw to monkey", ""));
            }
        }

        return monkeys;
    }

    private sealed class Monkey
    {
        public Queue<long> Items { get; set; } = new Queue<long>();
        public string Operation { get; set; } = "";
        public int DivisibleBy { get; set; }
        public int ThrowToMoneyIfDivisble { get; set; }
        public int ThrowToMonkeyIfNotDivisble { get; set; }
        public long InspectionCount { get; private set; }

        public long InspectNextItem(bool divideByThree)
        {
            InspectionCount++;

            if (Items.TryDequeue(out long currentWorryLevel))
            {
                var multiply = Operation.Contains('*');
                var operationParts = multiply
                    ? Operation.Split('*').Select(p => p.Trim()).ToArray()
                    : Operation.Split('+').Select(p => p.Trim()).ToArray();

                var left = operationParts[0] == "old" ? currentWorryLevel : int.Parse(operationParts[0]);
                var right = operationParts[1] == "old" ? currentWorryLevel : int.Parse(operationParts[1]);

                var divisor = divideByThree ? 3 : 1;

                if (multiply)
                {
                    return (left * right) / divisor;
                }

                return (left + right) / divisor;
            }

            throw new InvalidOperationException();
        }
    }
}
