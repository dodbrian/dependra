using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependra.Domain;

public class Project
{
    private readonly ISet<Package> _referencedPackages = new HashSet<Package>();
    private readonly IDictionary<string, Project> _referencedProjects = new Dictionary<string, Project>();

    public IReadOnlyList<Package> ReferencedPackages => _referencedPackages.ToList();
    public IReadOnlyList<Project> ReferencedProjects => _referencedProjects.Values.ToList();

    public Project(string fullPath)
    {
        FullPath = string.IsNullOrWhiteSpace(fullPath)
            ? throw new ArgumentOutOfRangeException(nameof(fullPath))
            : fullPath;
    }

    public string FullPath { get; }

    public void AddReferencedProject(Project referencedProject)
    {
        if (referencedProject is null) throw new ArgumentNullException(nameof(referencedProject));

        if (GetReferencedProjectByPath(referencedProject.FullPath) != null) return;

        _referencedProjects[referencedProject.FullPath] = referencedProject;
    }

    public void AddPackage(Package package)
    {
        if (package is null) throw new ArgumentNullException(nameof(package));

        _referencedPackages.Add(package);
    }

    private Project GetReferencedProjectByPath(string pathToProject)
    {
        return _referencedProjects.TryGetValue(pathToProject, out var project) ? project : null;
    }
}
