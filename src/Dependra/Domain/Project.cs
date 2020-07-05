using System;
using System.Collections.Generic;

namespace Dependra.Domain
{
    public class Project
    {
        private readonly IList<PackageReference> _packageReferences;
        private readonly IList<Project> _referencedProjects;

        public Project(string name, IList<Project> referencedProjects, IList<PackageReference> packageReferences)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentOutOfRangeException(nameof(name)) : name;
            _referencedProjects = referencedProjects ?? throw new ArgumentNullException(nameof(referencedProjects));
            _packageReferences = packageReferences ?? throw new ArgumentNullException(nameof(packageReferences));
        }

        public string Name { get; }

        public IReadOnlyList<Project> ReferencedProjects => _referencedProjects as IReadOnlyList<Project>;

        public IReadOnlyList<PackageReference> PackageReferences =>
            _packageReferences as IReadOnlyList<PackageReference>;
    }
}