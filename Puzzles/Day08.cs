namespace AdventOfCode2022.Puzzles;

internal class Day08 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day08.txt"))
            .Select(row => row.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(int[][] inputData)
    {
        var visibleCount = 0;

        for (int row = 0; row < inputData.Length; row++)
        {
            for (int col = 0; col < inputData[row].Length; col++)
            {
                if (IsVisible(inputData, row, col))
                {
                    visibleCount++;
                }
            }
        }

        return visibleCount.ToString();
    }

    private static string Part2(int[][] inputData)
    {
        var scores = new List<int>();

        for (int row = 0; row < inputData.Length; row++)
        {
            for (int col = 0; col < inputData[row].Length; col++)
            {
                scores.Add(CalculateScenicScore(inputData, row, col));
            }
        }

        return scores.Max().ToString();
    }

    private static bool IsVisible(int[][] inputData, int row, int col)
    {
        var value = inputData[row][col];

        if (col == 0 || row == 0)
        {
            return true;
        }

        var leftVisible = inputData[row].Where((_, i) => i > col).All(val => val < value);
        var rightVisible = inputData[row].Where((_, i) => i < col).All(val => val < value);

        bool topVisible()
        {
            for (int i = 0; i < row; i++)
            {
                if (inputData[i][col] >= value)
                {
                    return false;
                }
            }
            return true;
        }

        bool bottomVisible()
        {
            for (int i = row + 1; i < inputData.Length; i++)
            {
                if (inputData[i][col] >= value)
                {
                    return false;
                }
            }
            return true;
        }

        return leftVisible || rightVisible || topVisible() || bottomVisible();
    }

    private static int CalculateScenicScore(int[][] inputData, int row, int col)
    {
        var value = inputData[row][col];

        int getLeftScore()
        {
            var score = 0;

            if (col == 0)
            {
                return 0;
            }

            for (int i = col - 1; i >= 0; i--)
            {
                score++;
                if (inputData[row][i] >= value)
                {
                    break;
                }
            }

            return score;
        }

        int getRightScore()
        {
            var score = 0;

            if (col == inputData[col].Length - 1)
            {
                return 0;
            }

            for (int i = col + 1; i < inputData[col].Length; i++)
            {
                score++;
                if (inputData[row][i] >= value)
                {
                    break;
                }
            }

            return score;
        }

        int getTopScore()
        {
            var score = 0;

            if (row == 0)
            {
                return 0;
            }

            for (int i = row - 1; i >= 0; i--)
            {
                score++;
                if (inputData[i][col] >= value)
                {
                    break;
                }
            }

            return score;
        }

        int getBottomScore()
        {
            var score = 0;

            if (row == inputData.Length - 1)
            {
                return 0;
            }

            for (int i = row + 1; i < inputData.Length; i++)
            {
                score++;
                if (inputData[i][col] >= value)
                {
                    break;
                }
            }

            return score;
        }

        return getLeftScore() * getRightScore() * getTopScore() * getBottomScore();
    }
}
