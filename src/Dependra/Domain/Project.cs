using System;
using System.Collections.Generic;

namespace Dependra.Domain
{
    public class Project
    {
        private readonly IList<PackageReference> _packageReferences;
        private readonly IList<Project> _referencedProjects;

        public Project(string fullPath)
        {
            FullPath = string.IsNullOrWhiteSpace(fullPath)
                ? throw new ArgumentOutOfRangeException(nameof(fullPath))
                : fullPath;
        }

        public string FullPath { get; }

        public IReadOnlyList<Project> ReferencedProjects => _referencedProjects as IReadOnlyList<Project>;

        public IReadOnlyList<PackageReference> PackageReferences =>
            _packageReferences as IReadOnlyList<PackageReference>;
    }
}