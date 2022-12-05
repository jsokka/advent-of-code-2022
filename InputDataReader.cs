using System.ComponentModel;

namespace AdventOfCode2022;

public static class InputDataReader
{
    private readonly static string inputDataFolderPath =
        Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName, "InputData");

    internal async static Task<IEnumerable<T>> GetInputDataAsync<T>(string fileName, string delimiter = "\n", bool trimEmptyLastLine = true)
    {
        var lines = await GetRawInputDataAsync(fileName, delimiter, trimEmptyLastLine);

        return lines.Select(Convert<T>);
    }

    internal async static Task<IEnumerable<string>> GetRawInputDataAsync(string fileName, string delimiter = "\n", bool trimEmptyLastLine = true)
    {
        string filePath = Path.Combine(inputDataFolderPath, fileName);

        var lines = (await File.ReadAllTextAsync(filePath))
            .Split(delimiter).ToList();

        if (trimEmptyLastLine && string.IsNullOrWhiteSpace(lines.Last()))
        {
            lines.RemoveAt(lines.Count - 1);
        }

        return lines;
    }

    private static T Convert<T>(string? value)
    {
        var tc = TypeDescriptor.GetConverter(typeof(T));
        return (T)tc.ConvertFromInvariantString(value?.Trim() ?? "")!;
    }
}
