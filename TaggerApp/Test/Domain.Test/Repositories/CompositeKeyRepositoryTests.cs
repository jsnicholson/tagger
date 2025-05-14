using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class CompositeKeyRepositoryTests : BaseTest
{
    private ICompositeKeyRepository<TagOnFile, TagOnFileId> _repository = null!;

    [SetUp]
    public void Init()
    {
        _repository = new CompositeKeyRepository<TagOnFile, TagOnFileId>(DbContext, id => [id.TagId, id.FileId]);
        DbContext.SaveChanges();
    }

    [Test, CustomAutoData]
    public async Task GetByIdAsync_ReturnsCorrectEntity(Tag tag, File file)
    {
        DbContext.Tags.Add(tag);
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();
        
        var tagOnFile = new TagOnFile(tag.Id, file.Id);
        DbContext.TagsOnFiles.Add(tagOnFile);
        await DbContext.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(tagOnFile.Id);

        result.Should().NotBeNull();
        result!.TagId.Should().Be(tagOnFile.TagId);
        result.FileId.Should().Be(tagOnFile.FileId);
    }

    [Test, CustomAutoData]
    public async Task GetByIdsAsync_ReturnsCorrectEntities(List<Tag> tags, List<File> files)
    {
        DbContext.Tags.AddRange(tags);
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();
        
        var tagsOnFiles = tags.Zip(files, (t, f) => new TagOnFile(t.Id, f.Id)).ToList();
        DbContext.TagsOnFiles.AddRange(tagsOnFiles);
        await DbContext.SaveChangesAsync();

        var ids = tagsOnFiles.Select(t => t.Id).ToList();
        var results = (await _repository.GetByIdsAsync(ids)).ToList();

        results.Should().HaveCount(tagsOnFiles.Count);
        results.Select(t => t.Id).Should().BeEquivalentTo(ids);
    }

    [Test, CustomAutoData]
    public async Task DeleteByIdAsync_RemovesEntity( Tag tag, File file)
    {
        DbContext.Tags.Add(tag);
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();
        
        var tagOnFile = new TagOnFile(tag.Id, file.Id);
        DbContext.TagsOnFiles.Add(tagOnFile);
        await DbContext.SaveChangesAsync();

        await _repository.DeleteByIdAsync(tagOnFile.Id);

        var result = await DbContext.TagsOnFiles.FindAsync(tagOnFile.TagId, tagOnFile.FileId);
        result.Should().BeNull();
    }

    [Test]
    public async Task DeleteByIdAsync_ThrowsIfEntityNotFound()
    {
        var nonExistentId = new TagOnFileId(Guid.NewGuid(), Guid.NewGuid());

        var act = async () => await _repository.DeleteByIdAsync(nonExistentId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test, CustomAutoData]
    public async Task DeleteByIdsAsync_RemovesEntities(List<Tag> tags, List<File> files)
    {
        DbContext.Tags.AddRange(tags);
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();
        
        var tagsOnFiles = tags.Zip(files, (t, f) => new TagOnFile(t.Id, f.Id)).ToList();
        DbContext.TagsOnFiles.AddRange(tagsOnFiles);
        await DbContext.SaveChangesAsync();

        var ids = tagsOnFiles.Select(t => t.Id).ToList();
        await _repository.DeleteByIdsAsync(ids);

        var remaining = await DbContext.TagsOnFiles.ToListAsync();
        remaining.Should().BeEmpty();
    }
}
