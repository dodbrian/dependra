using System.Collections.Generic;
using System.IO;
using Dependra.Services.Contracts;

namespace Dependra.Services
{
    public class FileService : IFileService
    {
        public IEnumerable<string> GetFilesInDirectory(string pathToSolution, string searchPattern)
        {
            var projectPaths = Directory.GetFiles(pathToSolution, searchPattern, SearchOption.AllDirectories);

            return projectPaths;
        }

        public string GetAbsolutePath(string fullFilePath, string relativePath)
        {
            var directoryName = Path.GetDirectoryName(fullFilePath);
            var normalizedPath = EnsureProperDirectorySeparator(relativePath);
            var combinedPath = Path.Combine(directoryName ?? string.Empty, normalizedPath);
            var absolutePath = Path.GetFullPath(combinedPath);

            return absolutePath;
        }

        public string EnsureProperDirectorySeparator(string path)
        {
            var normalizedPath = path
                .Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            return normalizedPath;
        }
    }
}