using System.IO;
using Dependra.Domain;

namespace Dependra.Services
{
    public class SolutionLoader
    {
        public Solution LoadFromPath(string pathToSolution)
        {
            var projectPaths = Directory.GetFiles(pathToSolution, "*.csproj", SearchOption.AllDirectories);
            var solution = new Solution();

            foreach (var pathToProject in projectPaths)
            {
                solution.AddProject(pathToProject);
            }

            return solution;
        }
    }
}