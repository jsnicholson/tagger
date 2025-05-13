using Domain.Entities;
using FluentAssertions;
using File = Domain.Entities.File;

namespace Domain.Test.Entities;

public class TagOnFileValueTests : BaseTest {
    [Test]
    public void CanCreateTagOnFileValue()
    {
        // Arrange
        var file = new File("C:/example/file.txt");
        var tag = new Tag("rating");
        var tagOnFile = new TagOnFile(tag.Id, file.Id) { Tag = tag, File = file };

        var tagOnFileValue = new TagOnFileValue(file.Id, tag.Id, "5")
        {
            TagOnFile = tagOnFile
        };

        // Act
        DbContext.Add(file);
        DbContext.Add(tag);
        DbContext.Add(tagOnFile);
        DbContext.Add(tagOnFileValue);
        DbContext.SaveChanges();

        // Assert
        tagOnFileValue.TagOnFile.Should().NotBeNull();
        tagOnFileValue.Value.Should().Be("5");

        // Verify the data was saved correctly in the database
        var savedValue = DbContext.Set<TagOnFileValue>().Single();
        savedValue.Value.Should().Be("5");
        savedValue.FileId.Should().Be(file.Id);
        savedValue.TagId.Should().Be(tag.Id);
    }
}