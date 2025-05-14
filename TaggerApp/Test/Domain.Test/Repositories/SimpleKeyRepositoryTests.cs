using Domain.Repositories;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class SimpleKeyRepositoryTests : BaseTest {
    private ISimpleKeyRepository<File> _repository = null!;

    [SetUp]
    public void Init() {
        _repository = new SimpleKeyRepository<File>(DbContext);
    }

    [Test, CustomAutoData]
    public async Task GetByIdAsync_ReturnsCorrectEntity(File file) {
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(file.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(file.Id);
        result.Path.Should().Be(file.Path);
    }

    [Test, CustomAutoData]
    public async Task GetByIdsAsync_ReturnsCorrectEntities(List<File> files) {
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();

        var ids = files.Select(f => f.Id);
        var results = (await _repository.GetByIdsAsync(ids)).ToList();

        results.Should().HaveCount(files.Count);
        results.Select(f => f!.Id).Should().BeEquivalentTo(ids);
    }

    [Test, CustomAutoData]
    public async Task DeleteByIdAsync_RemovesEntity(File file) {
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();

        await _repository.DeleteByIdAsync(file.Id);

        var result = await DbContext.Files.FindAsync(file.Id);
        result.Should().BeNull();
    }

    [Test]
    public async Task DeleteByIdAsync_ThrowsIfEntityNotFound() {
        var nonExistentId = Guid.NewGuid();

        var act = async () => await _repository.DeleteByIdAsync(nonExistentId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test, CustomAutoData]
    public async Task DeleteByIdsAsync_RemovesEntities(List<File> files) {
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();

        var ids = files.Select(f => f.Id);
        await _repository.DeleteByIdsAsync(ids);

        var remaining = await DbContext.Files.ToListAsync();
        remaining.Should().BeEmpty();
    }
}
