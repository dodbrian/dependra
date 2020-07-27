using Dependra.Services;
using Dependra.Services.Contracts;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dependra.CoreTests
{
    public class When_loading_solution_as_folder
    {
        private const string TestPath = "TestPath";

        [Fact]
        public void Should_return_correct_solution()
        {
            // arrange
            var fileServiceMoq = new Mock<IFileService>();
            fileServiceMoq
                .Setup(service => service.GetFilesInDirectory(TestPath, It.IsAny<string>()))
                .Returns(new[]
                {
                    "/home/user/solution/project1/Project1.csproj",
                    "/home/user/solution/project2/Project2.csproj",
                    "/home/user/solution/project3/Project3.csproj"
                });

            var projectLoaderMoq = new Mock<IProjectLoader>();
            projectLoaderMoq
                .SetupSequence(loader => loader.GetReferencedProjectPaths())
                .Returns(new[]
                {
                    "/home/user/solution/project2/Project2.csproj",
                    "/home/user/solution/project3/Project3.csproj"
                })
                .Returns(new[]
                {
                    "/home/user/solution/project3/Project3.csproj"
                });

            projectLoaderMoq
                .SetupSequence(loader => loader.GetPackageReferences())
                .Returns(new[]
                {
                    ("Newtonsoft.Json", "12.0.3"),
                    ("Serilog", "2.10.0-dev-01213")
                })
                .Returns(new[]
                {
                    ("Newtonsoft.Json", "11.0.6"),
                    ("Microsoft.Extensions.DependencyInjection", "5.0.0-preview.7.20364.11")
                })
                .Returns(new[]
                {
                    ("Newtonsoft.Json", "12.0.3"),
                    ("Microsoft.Extensions.DependencyInjection", "5.0.0-preview.7.20364.11"),
                    ("Microsoft.EntityFrameworkCore", "5.0.0-preview.7.20365.15")
                });

            var solutionLoader = new SolutionLoader(fileServiceMoq.Object, projectLoaderMoq.Object);

            // act
            var solution = solutionLoader.LoadFromPath(TestPath);

            // assert
            solution.Path.Should().Be(TestPath);
            solution.NumberOfProjects.Should().Be(3, because: "three projects were referenced");
            solution.NumberOfPackages.Should().Be(5, because: "five packages were referenced");
        }
    }
}