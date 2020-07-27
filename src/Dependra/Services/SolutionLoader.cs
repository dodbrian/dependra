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
            var solution = new Solution(pathToSolution);
            var projectPaths = _fileService.GetFilesInDirectory(pathToSolution, CsProjectSearchPattern);

            foreach (var projectPath in projectPaths.EmptyIfNull())
            {
                _projectLoader.Load(projectPath);

                var project = new Project(projectPath);
                solution.AddProject(project);

                var referencedProjectPaths = _projectLoader.GetReferencedProjectPaths();

                foreach (var referencedProjectPath in referencedProjectPaths.EmptyIfNull())
                {
                    if (!solution.TryGetProject(referencedProjectPath, out var referencedProject))
                    {
                        referencedProject = new Project(referencedProjectPath);
                        solution.AddProject(referencedProject);
                    }

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
    }
}