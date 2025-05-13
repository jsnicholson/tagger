using Domain.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Domain.Test.Repositories;

[TestFixture]
public class GenericRepositoryTests : BaseTest
{
    private GenericRepository<File> _repository = null!;

    [SetUp]
    public void Init()
    {
        _repository = new GenericRepository<File>(DbContext);
    }

    [Test]
    public async Task AddAsync_AddsSingleEntity()
    {
        var file = new File("/test/single.txt");

        await _repository.AddAsync(file);

        var result = await DbContext.Files.FindAsync(file.Id);
        result.Should().NotBeNull();
        result!.Path.Should().Be("/test/single.txt");
    }

    [Test]
    public async Task AddAsync_AddsMultipleEntities()
    {
        var files = new[]
        {
            new File("/test/file1.txt"),
            new File("/test/file2.txt")
        };

        await _repository.AddAsync(files);

        var result = await DbContext.Files.ToListAsync();
        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsCorrectEntity()
    {
        var file = new File("/test/getbyid.txt");
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(file.Id);

        result.Should().NotBeNull();
        result!.Path.Should().Be("/test/getbyid.txt");
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        var files = new[]
        {
            new File("/all/one.txt"),
            new File("/all/two.txt")
        };
        DbContext.Files.AddRange(files);
        await DbContext.SaveChangesAsync();

        var result = (await _repository.GetAllAsync()).ToList();

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task UpdateAsync_UpdatesEntity()
    {
        var file = new File("/old/path.txt");
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();

        file.Path = "/new/path.txt";
        await _repository.UpdateAsync(file);

        var result = await DbContext.Files.FindAsync(file.Id);
        result!.Path.Should().Be("/new/path.txt");
    }

    [Test]
    public async Task DeleteAsync_RemovesEntity()
    {
        var file = new File("/to/delete.txt");
        DbContext.Files.Add(file);
        await DbContext.SaveChangesAsync();

        await _repository.DeleteAsync(file.Id);

        var result = await DbContext.Files.FindAsync(file.Id);
        result.Should().BeNull();
    }

    [Test]
    public async Task DeleteAsync_DoesNothingIfIdNotFound()
    {
        // Just ensure it doesn’t throw
        var nonExistentId = Guid.NewGuid();
        var action = async () => await _repository.DeleteAsync(nonExistentId);
        await action.Should().NotThrowAsync();
    }
}
