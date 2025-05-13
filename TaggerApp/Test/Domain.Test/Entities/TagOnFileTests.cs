using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;
using NUnit.Framework;

namespace Domain.Test.Entities;

[TestFixture]
public class TagOnFileTests
{
    private DbContextOptions<TestDbContext> _options = null!;
    private TestDbContext _context = null!;

    [SetUp]
    public void SetUp()
    {
        _options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        _context = new TestDbContext(_options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }

    [Test]
    public void Can_Create_TagOnFile_With_Value()
    {
        // Arrange
        var file = new File("example.txt");
        var tag = new Tag("rating");

        _context.Files.Add(file);
        _context.Tags.Add(tag);
        _context.SaveChanges();

        var tagOnFile = new TagOnFile(tag.Id, file.Id)
        {
            File = file,
            Tag = tag,
            Value = new TagOnFileValue(file.Id, tag.Id, "5")
        };

        _context.TagsOnFiles.Add(tagOnFile);
        _context.SaveChanges();

        // Act
        var loaded = _context.TagsOnFiles
            .Include(t => t.Tag)
            .Include(t => t.File)
            .Include(t => t.Value)
            .FirstOrDefault(t => t.FileId == file.Id && t.TagId == tag.Id);

        // Assert (FluentAssertions)
        loaded.Should().NotBeNull();
        loaded!.Tag.Name.Should().Be("rating");
        loaded.File.Path.Should().Be("example.txt");
        loaded.Value.Should().NotBeNull();
        loaded.Value!.Value.Should().Be("5");
    }
}