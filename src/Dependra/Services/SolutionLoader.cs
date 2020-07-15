using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dependra.Domain;

namespace Dependra.Services
{
    public class SolutionLoader
    {
        public Solution LoadFromPath(string pathToSolution)
        {
            var projectPaths = Directory.GetFiles(pathToSolution, "*.csproj", SearchOption.AllDirectories);
            var solution = new Solution();

            var projects = projectPaths
                .Select(path => new Project(path))
                .ToList();

            solution.AddProjectsRange(projects);

            foreach (var project in projects)
            {
                var projectXml = XDocument.Load(project.FullPath);
                var referencedProjectPaths = projectXml
                    .Descendants("ProjectReference")
                    .Select(element => element.Attribute("Include")?.Value)
                    .Where(path => path != null)
                    .Select(path => GetAbsolutePath(project.FullPath, path))
                    .ToList();

                foreach (var referencedProjectPath in referencedProjectPaths)
                {
                    var referencedProject = solution.GetProjectByPath(referencedProjectPath) ??
                                            new Project(referencedProjectPath);

                    project.AddReferencedProject(referencedProject);
                }

                var packageReferences = projectXml
                    .Descendants("PackageReference")
                    .Select(element => (element.Attribute("Include")?.Value, element.Attribute("Version")?.Value))
                    .ToList();

                foreach (var (packageName, packageVersion) in packageReferences)
                {
                    var package = new Package(packageName, packageVersion);
                    project.AddPackage(package);
                }
            }

            return solution;
        }

        private static string GetAbsolutePath(string fullFilePath, string path)
        {
            var directoryName = Path.GetDirectoryName(fullFilePath);
            var normalizedPath = NormalizePath(path);
            var combinedPath = Path.Combine(directoryName ?? string.Empty, normalizedPath);
            var absolutePath = Path.GetFullPath(combinedPath);

            return absolutePath;
        }

        private static string NormalizePath(string path)
        {
            var normalizedPath = path
                .Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);

            return normalizedPath;
        }
    }
}