using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class TagRepositoryTests : BaseTest
{
    private TagRepository _repository = null!;

    [SetUp]
    public void Init()
    {
        _repository = new TagRepository(DbContext);
    }

    [Test]
    public async Task GetTagsForFileAsync_ReturnsExpectedTags()
    {
        // Arrange
        var file = new File("/documents/report1.docx");
        var tag1 = new Tag("Work");
        var tag2 = new Tag("Personal");
        var tag3 = new Tag("Urgent");

        DbContext.Files.Add(file);
        DbContext.Tags.AddRange(tag1, tag2, tag3);
        await DbContext.SaveChangesAsync();

        DbContext.TagsOnFiles.AddRange(
            new TagOnFile(tag1.Id, file.Id),
            new TagOnFile(tag2.Id, file.Id)
        );
        await DbContext.SaveChangesAsync();

        // Act
        var result = (await _repository.GetTagsForFileAsync(file.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == tag1.Id);
        result.Should().Contain(t => t.Id == tag2.Id);
        result.Should().NotContain(t => t.Id == tag3.Id); // tag3 is not associated with the file
    }

    [Test]
    public async Task GetTagsForFileAsync_ReturnsEmpty_WhenNoTagsAreAssociated()
    {
        // Arrange
        var file = new File("/documents/report1.docx");
        var tag1 = new Tag("Work");

        DbContext.Files.Add(file);
        DbContext.Tags.Add(tag1);
        await DbContext.SaveChangesAsync();

        // Act
        var result = (await _repository.GetTagsForFileAsync(file.Id)).ToList();

        // Assert
        result.Should().BeEmpty(); // No tags are associated with this file
    }

    [Test]
    public async Task GetTagsForFileAsync_ReturnsTagsEvenIfNoFileTagsExist()
    {
        // Arrange
        var file = new File("/documents/report1.docx");

        // Act
        var result = (await _repository.GetTagsForFileAsync(file.Id)).ToList();

        // Assert
        result.Should().BeEmpty(); // No tags should be returned, since no tags are associated with this file
    }

    [Test]
    public async Task GetTagsForFileAsync_ReturnsTagsEvenIfMultipleFilesHaveTags()
    {
        // Arrange
        var file1 = new File("/documents/report1.docx");
        var file2 = new File("/documents/report2.docx");
        var tag1 = new Tag("Work");
        var tag2 = new Tag("Urgent");

        DbContext.Files.AddRange(file1, file2);
        DbContext.Tags.AddRange(tag1, tag2);
        await DbContext.SaveChangesAsync();

        DbContext.TagsOnFiles.AddRange(
            new TagOnFile(tag1.Id, file1.Id),
            new TagOnFile(tag2.Id, file1.Id)
        );
        await DbContext.SaveChangesAsync();

        // Act
        var result = (await _repository.GetTagsForFileAsync(file1.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == tag1.Id);
        result.Should().Contain(t => t.Id == tag2.Id);
    }
}
