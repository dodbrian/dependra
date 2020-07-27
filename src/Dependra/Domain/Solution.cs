using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependra.Domain
{
    public class Solution
    {
        private readonly IDictionary<string, Project> _projects = new Dictionary<string, Project>();
        private readonly IDictionary<(string, string), Package> _packages = new Dictionary<(string, string), Package>();

        public int NumberOfProjects => _projects.Count;
        public IReadOnlyList<Project> Projects => _projects.Values.ToList();

        public int NumberOfPackages => _packages.Count;

        public bool TryGetProject(string pathToProject, out Project project)
        {
            return _projects.TryGetValue(pathToProject, out project);
        }

        public void AddProject(Project project)
        {
            if (project is null) throw new ArgumentNullException(nameof(project));
            if (TryGetProject(project.FullPath, out _)) return;

            _projects[project.FullPath] = project;
        }

        public bool TryGetPackage(string packageName, string packageVersion, out Package package)
        {
            return _packages.TryGetValue((packageName, packageVersion), out package);
        }

        public void AddPackage(Package package)
        {
            if (package is null) throw new ArgumentNullException(nameof(package));
            if (TryGetPackage(package.Name, package.Version, out _)) return;

            _packages[(package.Name, package.Version)] = package;
        }
    }
}