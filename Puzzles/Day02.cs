using System.Diagnostics;

namespace AdventOfCode2022.Puzzles;

internal class Day02 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day02.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var totalScore = 0;

        for (int i = 0; i < inputData.Length; i++)
        {
            var round = inputData[i];
            var selections = round.Split(' ');
            var opponentsSelection = selections[0];
            var mySelection = selections[1];

            int score = mySelection switch
            {
                "X" when opponentsSelection == "C" => 1 + 6, // win
                "X" when opponentsSelection == "A" => 1 + 3, // draw
                "X" when opponentsSelection == "B" => 1 + 0, // lose
                "Y" when opponentsSelection == "A" => 2 + 6, // win
                "Y" when opponentsSelection == "B" => 2 + 3, // draw
                "Y" when opponentsSelection == "C" => 2 + 0, // lose
                "Z" when opponentsSelection == "B" => 3 + 6, // win
                "Z" when opponentsSelection == "C" => 3 + 3, // draw
                "Z" when opponentsSelection == "A" => 3 + 0, // lose
                _ => throw new UnreachableException()
            };

            totalScore += score;
        }

        return totalScore.ToString();
    }

    private static string Part2(string[] inputData)
    {
        var totalScore = 0;

        for (int i = 0; i < inputData.Length; i++)
        {
            var round = inputData[i];
            (string opponentsSelection, string neededEndResult) = GetColumns(round);

            int score = neededEndResult switch
            {
                "X" when opponentsSelection == "A" => 3 + 0, // lose
                "X" when opponentsSelection == "B" => 1 + 0, // lose
                "X" when opponentsSelection == "C" => 2 + 0, // lose
                "Y" when opponentsSelection == "A" => 1 + 3, // draw
                "Y" when opponentsSelection == "B" => 2 + 3, // draw
                "Y" when opponentsSelection == "C" => 3 + 3, // draw
                "Z" when opponentsSelection == "B" => 3 + 6, // win
                "Z" when opponentsSelection == "C" => 1 + 6, // win
                "Z" when opponentsSelection == "A" => 2 + 6, // win
                _ => throw new UnreachableException()
            };

            totalScore += score;
        }

        return totalScore.ToString();
    }

    private static (string, string) GetColumns(string round)
    {
        var selections = round.Split(' ');
        return (selections[0], selections[1]);
    }
}
