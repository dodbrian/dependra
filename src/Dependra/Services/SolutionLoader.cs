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

            foreach (var projectPath in projectPaths)
            {
                var project = new Project(projectPath);
                solution.AddProject(project);

                var projectXml = XDocument.Load(project.FullPath);
                var referencedProjectPaths = projectXml
                    .Descendants("ProjectReference")
                    .Select(element => element.Attribute("Include")?.Value)
                    .Where(path => path != null)
                    .Select(path => GetAbsolutePath(project.FullPath, path))
                    .ToList();

                foreach (var referencedProjectPath in referencedProjectPaths)
                {
                    if (!solution.TryGetProject(referencedProjectPath, out var referencedProject))
                    {
                        referencedProject = new Project(referencedProjectPath);
                        solution.AddProject(referencedProject);
                    }

                    project.AddReferencedProject(referencedProject);
                }

                var packageReferences = projectXml
                    .Descendants("PackageReference")
                    .Select(element => (element.Attribute("Include")?.Value, element.Attribute("Version")?.Value))
                    .ToList();

                foreach (var (packageName, packageVersion) in packageReferences)
                {
                    if (!solution.TryGetPackage(packageName, packageVersion, out var package))
                    {
                        package = new Package(packageName, packageVersion);
                        solution.AddPackage(package);
                    }
                    
                    project.AddPackage(package);
                }
            }

            return solution;
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