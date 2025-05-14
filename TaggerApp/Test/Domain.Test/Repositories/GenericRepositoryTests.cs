using Domain.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class GenericRepositoryTests : BaseTest
{
    private IGenericRepository<File> _repository = null!;

    [SetUp]
    public void Init()
    {
        _repository = new GenericRepository<File>(DbContext);
    }

    [Test, CustomAutoData]
    public async Task AddAsync_AddsSingleEntity(File file)
    {
        await _repository.AddAsync(file);

        var result = await DbContext.Files.FindAsync(file.Id);
        result.Should().NotBeNull();
        result.Path.Should().Be(file.Path);
    }

    [Test, CustomAutoData]
    public async Task AddAsync_AddsMultipleEntities(List<File> files)
    {
        await _repository.AddAsync(files);

        var result = await DbContext.Files.ToListAsync();
        result.Should().HaveCount(files.Count);
    }

    [Test, CustomAutoData]
    public async Task GetAllAsync_ReturnsAllEntities(List<File> files)
    {
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();

        var result = (await _repository.GetAllAsync()).ToList();

        result.Should().HaveCount(files.Count);
        result.Select(f => f.Id).Should().BeEquivalentTo(files.Select(f => f.Id));
    }

    [Test, CustomAutoData]
    public async Task UpdateAsync_UpdatesEntity(File file, string newPath)
    {
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();

        file.Path = newPath;
        await _repository.UpdateAsync(file);

        var result = await DbContext.Files.FindAsync(file.Id);
        result!.Path.Should().Be(newPath);
    }

    [Test, CustomAutoData]
    public async Task UpdateAsync_UpdatesMultipleEntities(List<File> files, string updatedPath)
    {
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();

        foreach (var file in files)
            file.Path = updatedPath;

        await _repository.UpdateAsync(files);

        var updated = await DbContext.Files.ToListAsync();
        updated.Should().OnlyContain(f => f.Path == updatedPath);
    }

    [Test, CustomAutoData]
    public async Task DeleteAsync_RemovesEntity(File file)
    {
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();

        await _repository.DeleteAsync(file);

        var result = await DbContext.Files.FindAsync(file.Id);
        result.Should().BeNull();
    }

    [Test, CustomAutoData]
    public async Task DeleteAsync_RemovesMultipleEntities(List<File> files)
    {
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();

        await _repository.DeleteAsync(files);

        var remaining = await DbContext.Files.ToListAsync();
        remaining.Should().BeEmpty();
    }
}
