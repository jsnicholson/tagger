using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Domain.Test.Entities;

[TestFixture]
public class TagOnFileTests {
    private DbContextOptions<TestDbContext> _options = null!;
    private TestDbContext _context = null!;

    [SetUp]
    public void SetUp() {
        _options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        _context = new TestDbContext(_options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown() {
        _context.Database.CloseConnection();
        _context.Dispose();
    }

    [Test, CustomAutoData]
    public void Can_Create_TagOnFile(File file, Tag tag, string value) {
        // Arrange
        _context.Files.Add(file);
        _context.Tags.Add(tag);
        _context.SaveChanges();

        var tagOnFile = new TagOnFile(tag.Id, file.Id) {
            File = file,
            Tag = tag,
        };

        _context.TagsOnFiles.Add(tagOnFile);
        _context.SaveChanges();

        // Act
        var loaded = _context.TagsOnFiles
            .Include(t => t.Tag)
            .Include(t => t.File)
            .FirstOrDefault(t => t.FileId == file.Id && t.TagId == tag.Id);

        // Assert (FluentAssertions)
        loaded.Should().NotBeNull();
        loaded.Tag.Name.Should().Be(tag.Name);
        loaded.File.Path.Should().Be(file.Path);
    }
}
