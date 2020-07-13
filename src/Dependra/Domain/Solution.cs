using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependra.Domain
{
    public class Solution
    {
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        
        public int NumberOfProjects => _projects.Count;
        public IReadOnlyList<Project> Projects => _projects.Values.ToList();

        public void AddProject(Project project)
        {
            if (project is null) throw new ArgumentNullException(nameof(project));
            if (GetProjectByPath(project.FullPath) != null) return;

            _projects[project.FullPath] = project;
        }

        public void AddProjectsRange(IEnumerable<Project> projects)
        {
            foreach (var project in projects)
            {
                AddProject(project);
            }
        }

        public Project GetProjectByPath(string pathToProject)
        {
            return _projects.TryGetValue(pathToProject, out var existingProject) ? existingProject : null;
        }
    }
}