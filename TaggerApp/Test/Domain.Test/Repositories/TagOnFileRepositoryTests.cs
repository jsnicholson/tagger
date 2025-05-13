using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class TagOnFileRepositoryTests : BaseTest
{
    private TagOnFileRepository _repository = null!;
    private File _file = null!;
    private Tag _tag = null!;

    [SetUp]
    public void Init()
    {
        _repository = new TagOnFileRepository(DbContext);
        _file = new File("/documents/report1.docx");
        _tag = new Tag("Work");

        DbContext.Files.Add(_file);
        DbContext.Tags.Add(_tag);
        DbContext.SaveChanges();
    }

    [Test]
    public async Task AddTagToFileAsync_AddsTagToFileCorrectly()
    {
        // Act
        await _repository.AddTagToFileAsync(_tag.Id, _file.Id);

        // Assert
        var result = await _repository.GetTagOnFileAsync(_tag.Id, _file.Id);
        result.Should().NotBeNull();
        result.TagId.Should().Be(_tag.Id);
        result.FileId.Should().Be(_file.Id);
    }

    [Test]
    public async Task AddTagsToFileAsync_AddsMultipleTagsToFileCorrectly()
    {
        // Arrange
        var tag1 = new Tag("Work");
        var tag2 = new Tag("Personal");
        DbContext.Tags.AddRange(tag1, tag2);
        await DbContext.SaveChangesAsync();

        // Act
        await _repository.AddTagsToFileAsync([tag1.Id, tag2.Id], _file.Id);

        // Assert
        var result1 = await _repository.GetTagOnFileAsync(tag1.Id, _file.Id);
        var result2 = await _repository.GetTagOnFileAsync(tag2.Id, _file.Id);
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
    }

    [Test]
    public async Task AddTagsToFilesAsync_AddsTagsToMultipleFilesCorrectly()
    {
        // Arrange
        var file2 = new File("/documents/report2.docx");
        DbContext.Files.Add(file2);
        await DbContext.SaveChangesAsync();

        var tags = new List<Guid> { _tag.Id };
        var fileIds = new List<(Guid tagId, Guid fileId)>
        {
            (_tag.Id, _file.Id),
            (_tag.Id, file2.Id)
        };

        // Act
        await _repository.AddTagsToFilesAsync(fileIds);

        // Assert
        var result1 = await _repository.GetTagOnFileAsync(_tag.Id, _file.Id);
        var result2 = await _repository.GetTagOnFileAsync(_tag.Id, file2.Id);
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
    }

    [Test]
    public async Task RemoveTagFromFileAsync_RemovesTagFromFileCorrectly()
    {
        // Arrange
        await _repository.AddTagToFileAsync(_tag.Id, _file.Id);

        // Act
        await _repository.RemoveTagFromFileAsync(_tag.Id, _file.Id);

        // Assert
        var result = await _repository.GetTagOnFileAsync(_tag.Id, _file.Id);
        result.Should().BeNull();
    }

    [Test]
    public async Task RemoveTagsFromFileAsync_RemovesMultipleTagsFromFileCorrectly()
    {
        // Arrange
        var tag2 = new Tag("Personal");
        DbContext.Tags.Add(tag2);
        await DbContext.SaveChangesAsync();

        await _repository.AddTagsToFileAsync([_tag.Id, tag2.Id], _file.Id);

        // Act
        await _repository.RemoveTagsFromFileAsync([_tag.Id, tag2.Id], _file.Id);

        // Assert
        var result1 = await _repository.GetTagOnFileAsync(_tag.Id, _file.Id);
        var result2 = await _repository.GetTagOnFileAsync(tag2.Id, _file.Id);
        result1.Should().BeNull();
        result2.Should().BeNull();
    }

    [Test]
    public async Task RemoveTagsFromFilesAsync_RemovesTagsFromMultipleFilesCorrectly()
    {
        // Arrange
        var file2 = new File("/documents/report2.docx");
        DbContext.Files.Add(file2);
        await DbContext.SaveChangesAsync();

        var tag2 = new Tag("Personal");
        DbContext.Tags.Add(tag2);
        await DbContext.SaveChangesAsync();

        await _repository.AddTagsToFilesAsync(new List<(Guid, Guid)>
        {
            (_tag.Id, _file.Id),
            (_tag.Id, file2.Id),
            (tag2.Id, _file.Id),
            (tag2.Id, file2.Id)
        });

        // Act
        await _repository.RemoveTagsFromFilesAsync(new List<(Guid, Guid)>
        {
            (_tag.Id, _file.Id),
            (tag2.Id, file2.Id)
        });

        // Assert
        var result1 = await _repository.GetTagOnFileAsync(_tag.Id, _file.Id);
        var result2 = await _repository.GetTagOnFileAsync(tag2.Id, file2.Id);
        var result3 = await _repository.GetTagOnFileAsync(_tag.Id, file2.Id);
        var result4 = await _repository.GetTagOnFileAsync(tag2.Id, _file.Id);

        result1.Should().BeNull();
        result2.Should().BeNull();
        result3.Should().NotBeNull();
        result4.Should().NotBeNull();
    }
}