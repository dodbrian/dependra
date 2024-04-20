using System;

namespace Dependra.Domain;

public class Package
{
    public string Name { get; }
    public string Version { get; }

    public Package(string name, string version)
    {
        Version = version != null && version.Trim() == string.Empty
            ? throw new ArgumentOutOfRangeException(nameof(version))
            : version;

        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentOutOfRangeException(nameof(name)) : name;
    }
}