using System.Collections.Generic;

namespace Dependra.Services.Contracts;

public interface IFileService
{
    IEnumerable<string> GetFilesInDirectory(string pathToSolution, string searchPattern);
    string GetAbsolutePath(string fullFilePath, string relativePath);
    string EnsureProperDirectorySeparator(string path);
    string GetAbsolutePathBasedOnCurrent(string relativePath);
}