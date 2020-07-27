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
    }
}