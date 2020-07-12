using System.IO;
using System.Linq;
using Dependra.Domain;

namespace Dependra.Services
{
    public class SolutionLoader
    {
        public Solution LoadFromPath(string pathToSolution)
        {
            var projectPaths = Directory.GetFiles(pathToSolution, "*.csproj", SearchOption.AllDirectories);
            var solution = new Solution();

            var projects = projectPaths.Select(path => new Project(path));
            solution.AddProjectsRange(projects);

            return solution;
        }
    }
}