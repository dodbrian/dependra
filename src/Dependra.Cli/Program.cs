﻿using System;
using System.IO;
using System.Linq;
using Dependra.Services;

namespace Dependra.Cli
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            string pathToSolution;

            if (!args.Any() || string.IsNullOrWhiteSpace(args[0]))
            {
                pathToSolution = Directory.GetCurrentDirectory();
            }
            else
            {
                pathToSolution = args[0];
            }

            var solutionLoader = new SolutionLoader();
            var solution = solutionLoader.LoadFromPath(pathToSolution);

            Console.WriteLine($"Number of projects in solution: {solution.NumberOfProjects}");
        }
    }
}