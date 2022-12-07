using System.Diagnostics;

namespace AdventOfCode2022.Puzzles;

internal class Day07 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day07.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var currentDirectory = new Directory() { Path = "/" };
        var directories = ReadOutputs(inputData, currentDirectory);

        return directories.Where(d => d.Size <= 100_000).Sum(d => d.Size).ToString();
    }

    private static string Part2(string[] inputData)
    {
        var rootDirectory = new Directory() { Path = "/" };
        var currentDirectory = rootDirectory;
        var directories = ReadOutputs(inputData, currentDirectory);

        var requiredSpace = 30_000_000 - (70_000_000 - rootDirectory.Size);

        return directories.Where(d => d.Size >= requiredSpace).Min(d => d.Size).ToString();
    }

    private static IReadOnlyList<Directory> ReadOutputs(string[] outputData, Directory currentDirectory)
    {
        List<Directory> directories = new();
        
        foreach (var output in outputData)
        {
            if (output.StartsWith("$ cd"))
            {
                var dir = output.Replace("$ cd", "").Trim();

                if (dir == "/")
                {
                    continue;
                }

                if (dir == "..")
                {
                    currentDirectory = currentDirectory.ParentDirectory
                        ?? throw new UnreachableException();
                    continue;
                }
                
                currentDirectory = currentDirectory.Directories
                    .Single(d => d.Path.Split("/").Last() == dir);
            }
            else if (output.StartsWith("dir"))
            {
                var directory = new Directory
                {
                    Path = output.Replace("dir", "").Trim(),
                    ParentDirectory = currentDirectory
                };

                currentDirectory.Directories.Add(directory);
                directories.Add(directory);
            }
            else if (!output.StartsWith("$"))
            {
                var fileOuputParts = output.Split(' ');

                currentDirectory.Files.Add(new File
                {
                    Size = Convert.ToInt64(fileOuputParts[0]),
                    Name = fileOuputParts[1]
                });
            }
        }

        return directories;
    }

    private sealed class Directory
    {
        public string Path { get; set; } = null!;

        public Directory? ParentDirectory { get; set; }

        public List<Directory> Directories { get; set; } = new List<Directory>();

        public List<File> Files { get; set; } = new List<File>();

        public long Size => Files.Sum(f => f.Size) + Directories.Sum(d => d.Size);
    }

    private sealed class File
    {
        public string Name { get; set; } = "";

        public long Size { get; set; } = 0;
    }
}
