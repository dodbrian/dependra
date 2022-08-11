using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Dependra.Services.Contracts;

namespace Dependra.Services
{
    public class ProjectLoader : IProjectLoader
    {
        private readonly IFileService _fileService;

        private XDocument _projectXml;
        private string _projectFullPath;

        public ProjectLoader(IFileService fileService) => _fileService = fileService;

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
                .Select(path => _fileService.GetAbsolutePath(_projectFullPath, path))
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
    }
}