using System.IO;
using Dependra.Common;
using Dependra.Domain;
using Dependra.Services.Contracts;

namespace Dependra.Services
{
    public class SolutionLoader
    {
        private readonly IFileService _fileService;
        private readonly IProjectLoader _projectLoader;

        private const string CsProjectSearchPattern = "*.csproj";

        public SolutionLoader(IFileService fileService, IProjectLoader projectLoader)
        {
            _fileService = fileService;
            _projectLoader = projectLoader;
        }

        public Solution LoadFromPath(string pathToSolution)
        {
            // todo Extract to FileService and write tests
            if (!Path.IsPathRooted(pathToSolution))
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                pathToSolution = Path.Combine(currentDirectory, pathToSolution);
                pathToSolution = Path.GetFullPath(pathToSolution);
            }

            var solution = new Solution(pathToSolution);
            var projectPaths = _fileService.GetFilesInDirectory(pathToSolution, CsProjectSearchPattern);

            foreach (var projectPath in projectPaths.EmptyIfNull())
            {
                _projectLoader.Load(projectPath);

                var project = GetOrAddProject(solution, projectPath);
                var referencedProjectPaths = _projectLoader.GetReferencedProjectPaths();

                foreach (var referencedProjectPath in referencedProjectPaths.EmptyIfNull())
                {
                    var referencedProject = GetOrAddProject(solution, referencedProjectPath);
                    project.AddReferencedProject(referencedProject);
                }

                var packageReferences = _projectLoader.GetPackageReferences();

                foreach (var (packageName, packageVersion) in packageReferences.EmptyIfNull())
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

        private static Project GetOrAddProject(Solution solution, string projectPath)
        {
            if (solution.TryGetProject(projectPath, out var project)) return project;

            project = new Project(projectPath);
            solution.AddProject(project);

            return project;
        }
    }
}