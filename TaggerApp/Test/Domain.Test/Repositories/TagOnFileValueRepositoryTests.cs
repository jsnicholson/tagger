using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class TagOnFileValueRepositoryTests : BaseTest
{
    private TagOnFileValueRepository _repository = null!;
    private File _file = null!;
    private Tag _tag = null!;

    [SetUp]
    public void Init()
    {
        _repository = new TagOnFileValueRepository(DbContext);
        _file = new File("/documents/report1.docx");
        _tag = new Tag("Work");

        DbContext.Files.Add(_file);
        DbContext.Tags.Add(_tag);
        DbContext.SaveChanges();
    }

    [Test]
    public async Task AddTagValueToFileAsync_AddsValueCorrectly()
    {
        // Arrange
        const string value = "Urgent";

        // Act
        await _repository.AddValueToTagOnFileAsync(_file.Id, _tag.Id, value);

        // Assert
        var result = await _repository.GetTagValueForFileAsync(_file.Id, _tag.Id);
        result.Should().NotBeNull();
        result.Value.Should().Be(value);
    }

    [Test]
    public async Task RemoveTagValueFromFileAsync_RemovesValueCorrectly()
    {
        // Arrange
        var value = "Urgent";
        await _repository.AddValueToTagOnFileAsync(_file.Id, _tag.Id, value);

        // Act
        await _repository.RemoveTagValueFromFileAsync(_file.Id, _tag.Id);

        // Assert
        var result = await _repository.GetTagValueForFileAsync(_file.Id, _tag.Id);
        result.Should().BeNull();
    }

    [Test]
    public async Task GetValuesForFileAsync_ReturnsValuesCorrectly()
    {
        // Arrange
        var value1 = "Urgent";
        var value2 = "Important";
        await _repository.AddValueToTagOnFileAsync(_file.Id, _tag.Id, value1);
        await _repository.AddValueToTagOnFileAsync(_file.Id, _tag.Id, value2);

        // Act
        var result = (await _repository.GetValuesForFileAsync(_file.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(v => v.Value == value1);
        result.Should().Contain(v => v.Value == value2);
    }

    [Test]
    public async Task GetValuesForTagAsync_ReturnsValuesCorrectly()
    {
        // Arrange
        var value1 = "Urgent";
        var value2 = "Important";
        await _repository.AddValueToTagOnFileAsync(_file.Id, _tag.Id, value1);
        await _repository.AddValueToTagOnFileAsync(_file.Id, _tag.Id, value2);

        // Act
        var result = (await _repository.GetValuesForTagAsync(_tag.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(v => v.Value == value1);
        result.Should().Contain(v => v.Value == value2);
    }
}