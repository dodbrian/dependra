using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dependra.Services.Contracts;

namespace Dependra.Services
{
    public class ProjectLoader : IProjectLoader
    {
        private XDocument _projectXml;
        private string _projectFullPath;

        public void Load(string projectFullPath)
        {
            _projectFullPath = projectFullPath;
            _projectXml = XDocument.Load(projectFullPath);
        }

        public IEnumerable<string> GetReferencedProjectPaths()
        {
            var paths = _projectXml
                .Descendants("ProjectReference")
                .Select(element => element.Attribute("Include")?.Value)
                .Where(path => path != null)
                .Select(path => GetAbsolutePath(_projectFullPath, path))
                .ToList();

            return paths;
        }

        public IEnumerable<(string, string)> GetPackageReferences()
        {
            var packageReferences = _projectXml
                .Descendants("PackageReference")
                .Select(element => (element.Attribute("Include")?.Value, element.Attribute("Version")?.Value))
                .ToList();

            return packageReferences;
        }

        private static string GetAbsolutePath(string fullFilePath, string relativePath)
        {
            var directoryName = Path.GetDirectoryName(fullFilePath);
            var normalizedPath = EnsureProperDirectorySeparator(relativePath);
            var combinedPath = Path.Combine(directoryName ?? string.Empty, normalizedPath);
            var absolutePath = Path.GetFullPath(combinedPath);

            return absolutePath;
        }

        private static string EnsureProperDirectorySeparator(string path)
        {
            var normalizedPath = path
                .Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            return normalizedPath;
        }
   }
}