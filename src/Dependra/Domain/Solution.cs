using System.Collections.Generic;

namespace Dependra.Domain
{
    public class Solution
    {
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        public int NumberOfProjects => _projects.Count;

        public void AddProject(string pathToProject)
        {
            if (_projects.TryGetValue(pathToProject, out _)) return;

            _projects[pathToProject] = new Project(pathToProject);
        }
    }
}