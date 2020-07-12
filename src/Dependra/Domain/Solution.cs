using System.Collections.Generic;

namespace Dependra.Domain
{
    public class Solution
    {
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        public int NumberOfProjects => _projects.Count;

        public void AddProject(Project project)
        {
            if (_projects.TryGetValue(project.FullPath, out _)) return;

            _projects[project.FullPath] = project;
        }

        public void AddProjectsRange(IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                AddProject(project);
            }
        }
    }
}