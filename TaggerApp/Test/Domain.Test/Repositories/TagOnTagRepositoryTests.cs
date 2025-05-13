using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;

namespace Domain.Test.Repositories;

[TestFixture]
public class TagOnTagRepositoryTests : BaseTest
{
    private TagOnTagRepository _repository = null!;
    private Tag _tagger = null!;
    private Tag _tagged = null!;

    [SetUp]
    public void Init()
    {
        _repository = new TagOnTagRepository(DbContext);
        _tagger = new Tag("Tagger");
        _tagged = new Tag("Tagged");

        DbContext.Tags.AddRange(_tagger, _tagged);
        DbContext.SaveChanges();
    }

    [Test]
    public async Task AddTagToTagAsync_AddsTagCorrectly()
    {
        // Act
        await _repository.AddTagOnTagAsync(_tagger.Id, _tagged.Id);

        // Assert
        var result = await _repository.GetTagOnTagAsync(_tagger.Id, _tagged.Id);
        result.Should().NotBeNull();
        result.TaggerId.Should().Be(_tagger.Id);
        result.TaggedId.Should().Be(_tagged.Id);
    }

    [Test]
    public async Task RemoveTagFromTagAsync_RemovesTagCorrectly()
    {
        // Arrange
        await _repository.AddTagOnTagAsync(_tagger.Id, _tagged.Id);

        // Act
        await _repository.RemoveTagFromTagAsync(_tagger.Id, _tagged.Id);

        // Assert
        var result = await _repository.GetTagOnTagAsync(_tagger.Id, _tagged.Id);
        result.Should().BeNull();
    }

    [Test]
    public async Task GetTagsForTaggerAsync_ReturnsCorrectTags()
    {
        // Arrange
        await _repository.AddTagOnTagAsync(_tagger.Id, _tagged.Id);
        var anotherTagged = new Tag("AnotherTagged");
        DbContext.Tags.Add(anotherTagged);
        await DbContext.SaveChangesAsync();
        await _repository.AddTagOnTagAsync(_tagger.Id, anotherTagged.Id);

        // Act
        var result = (await _repository.GetTagsForTaggerAsync(_tagger.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.TaggedId == _tagged.Id);
        result.Should().Contain(t => t.TaggedId == anotherTagged.Id);
    }

    [Test]
    public async Task GetTagsForTaggedAsync_ReturnsCorrectTags()
    {
        // Arrange
        await _repository.AddTagOnTagAsync(_tagger.Id, _tagged.Id);
        var anotherTagger = new Tag("AnotherTagger");
        DbContext.Tags.Add(anotherTagger);
        await DbContext.SaveChangesAsync();
        await _repository.AddTagOnTagAsync(anotherTagger.Id, _tagged.Id);

        // Act
        var result = (await _repository.GetTagsForTaggedAsync(_tagged.Id)).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.TaggerId == _tagger.Id);
        result.Should().Contain(t => t.TaggerId == anotherTagger.Id);
    }
}