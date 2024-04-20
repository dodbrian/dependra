using System.Collections.Generic;

namespace Dependra.Services.Contracts;

public interface IProjectLoader
{
    void Load(string projectFullPath);
    IEnumerable<string> GetReferencedProjectPaths();
    IEnumerable<(string, string)> GetPackageReferences();
}