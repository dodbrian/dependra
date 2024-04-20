using Dependra.Services;
using FluentAssertions;
using Xunit;

namespace Dependra.CoreTests;

public class When_converting_relative_path
{
    [Theory]
    [InlineData("/home/user/app/src/common/main1.txt", "../../data/data1.txt", "/home/user/app/data/data1.txt")]
    public void Should_return_valid_absolute_path(string basePath, string relativePath, string expectedPath)
    {
        // arrange
        var fileService = new FileService();

        // act
        var absolutePath = fileService.GetAbsolutePath(basePath, relativePath);

        // assert
        absolutePath.Should().Be(expectedPath);
    }
}