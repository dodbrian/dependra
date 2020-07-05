using System;

namespace Dependra.Domain
{
    public class PackageReference
    {
        public string Name { get; }

        public PackageReference(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentOutOfRangeException(nameof(name)) : name;
        }
    }
}