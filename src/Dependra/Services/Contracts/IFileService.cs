using System.Collections.Generic;

namespace Dependra.Services.Contracts
{
    public interface IFileService
    {
        IEnumerable<string> GetFilesInDirectory(string pathToSolution, string searchPattern);
    }
}