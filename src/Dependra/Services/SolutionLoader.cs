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
                var referencedProjectPaths = projectXml.Root?
                    .Descendants("ProjectReference")
                    .Select(element => element.Attribute("Include")?.Value)
                    .Where(path => path != null)
                    .Select(path => GetAbsolutePath(project, path))
                    .ToList();

                foreach (var referencedProjectPath in referencedProjectPaths)
                {
                    var referencedProject = solution.GetProjectByPath(referencedProjectPath) ??
                                            new Project(referencedProjectPath);

                    project.AddReferencedProject(referencedProject);
                }
            }

            return solution;
        }

        private static string GetAbsolutePath(Project project, string path)
        {
            var directoryName = Path.GetDirectoryName(project.FullPath);
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