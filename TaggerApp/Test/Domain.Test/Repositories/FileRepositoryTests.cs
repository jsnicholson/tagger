using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class FileRepositoryTests : BaseTest
{
    private FileRepository _repository = null!;

    [SetUp]
    public void Init()
    {
        _repository = new FileRepository(DbContext);
    }

    [Test]
    public async Task GetFilesByTagAsync_ReturnsExpectedFiles()
    {
        // Arrange
        var tag = new Tag("Work");
        var file1 = new File("/documents/report1.docx");
        var file2 = new File("/documents/report2.docx");
        var unrelatedFile = new File("/music/song.mp3");

        DbContext.Tags.Add(tag);
        DbContext.Files.AddRange(file1, file2, unrelatedFile);
        await DbContext.SaveChangesAsync();

        DbContext.TagsOnFiles.AddRange(
            new TagOnFile(tag.Id, file1.Id),
            new TagOnFile(tag.Id, file2.Id)
        );
        await DbContext.SaveChangesAsync();

        // Act
        var result = (await _repository.GetFilesByTagAsync(tag.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(f => f.Id == file1.Id);
        result.Should().Contain(f => f.Id == file2.Id);
        result.Should().NotContain(f => f.Id == unrelatedFile.Id);
    }

    // Add more specific tests for FileRepository methods here
}