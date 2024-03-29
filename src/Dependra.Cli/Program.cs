﻿using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dependra.Services;

namespace Dependra.Cli;

internal static class Program
{
    private static void Main(string[] args)
    {
        string pathToSolution;

        if (!args.Any() || string.IsNullOrWhiteSpace(args[0]))
            pathToSolution = Directory.GetCurrentDirectory();
        else
            pathToSolution = args[0];

        var fileService = new FileService();
        var solutionLoader = new SolutionLoader(fileService, new ProjectLoader(fileService));
        var solution = solutionLoader.LoadFromPath(pathToSolution);

        Console.WriteLine($"Solution folder: {solution.Path}");
        Console.WriteLine($"Number of projects in solution: {solution.NumberOfProjects}");
        Console.WriteLine($"Number of packages in solution: {solution.NumberOfPackages}");

        var projection = solution.Projects
            .Select(
                project => new
                {
                    project.FullPath,
                    Packages = project.ReferencedPackages.Select(package => $"{package.Name}, {package.Version}"),
                    Projects = project.ReferencedProjects.Select(refProject => refProject.FullPath)
                });

        var json = JsonSerializer.Serialize(
            projection,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        Console.WriteLine(json);
    }
}
