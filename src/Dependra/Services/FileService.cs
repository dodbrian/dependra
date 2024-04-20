using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Dependra.Services.Contracts;

namespace Dependra.Services;

public partial class FileService : IFileService
{
    public IEnumerable<string> GetFilesInDirectory(string pathToSolution, string searchPattern)
    {
        var projectPaths = Directory.GetFiles(pathToSolution, searchPattern, SearchOption.AllDirectories);

        return projectPaths;
    }

    public string GetAbsolutePath(string fullFilePath, string relativePath)
    {
        var basePath = Path.GetDirectoryName(fullFilePath);
        var normalizedRelativePath = EnsureProperDirectorySeparator(relativePath ?? string.Empty);
        var combinedPath = Path.Combine(basePath ?? string.Empty, normalizedRelativePath);
        var absolutePath = Path.GetFullPath(combinedPath);

        return absolutePath;
    }

    public string EnsureProperDirectorySeparator(string path)
    {
        var normalizedPath = MyRegex().Replace(path, Path.DirectorySeparatorChar.ToString());

        return normalizedPath;
    }

    public string GetAbsolutePathBasedOnCurrent(string relativePath)
    {
        if (Path.IsPathRooted(relativePath)) return relativePath;

        var currentDirectory = Directory.GetCurrentDirectory();
        relativePath = Path.Combine(currentDirectory, relativePath);
        relativePath = Path.GetFullPath(relativePath);

        return relativePath;
    }

    [GeneratedRegex(@"((\\)+)|(\/+)")]
    private static partial Regex MyRegex();
}
