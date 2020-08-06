using System.IO;
using Dependra.Services;
using FluentAssertions;
using Xunit;

namespace Dependra.CoreTests
{
    public class When_any_path_passed
    {
        [Theory]
        [InlineData(@"/home\user/src\wrong\path1", ".home.user.src.wrong.path1")]
        [InlineData(@"/home/user/src/wrong/path2", ".home.user.src.wrong.path2")]
        [InlineData(@"/home/user///src\\\wrong/path3", ".home.user.src.wrong.path3")]
        public void Should_return_correct_path(string path, string expected)
        {
            // arrange
            var fileService = new FileService();
            var pathChar = Path.DirectorySeparatorChar;
            expected = expected.Replace('.', pathChar);

            // act
            var normalizedPath = fileService.EnsureProperDirectorySeparator(path);

            // assert
            normalizedPath.Should().Be(expected);
        }
    }
}