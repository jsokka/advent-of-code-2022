namespace AdventOfCode2022.Puzzles
{
    internal interface IPuzzle
    {
        Task<(string, string)> Solve();
    }
}
