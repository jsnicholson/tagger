using Domain.Entities;

using FluentAssertions;

using File = Domain.Entities.File;

namespace Domain.Test.Entities;

public class TagOnFileValueTests : BaseTest {
    [Test, CustomAutoData]
    public async Task CanCreateTagOnFileValue(Tag tag, File file, string value) {
        var tagOnFile = new TagOnFile(tag.Id, file.Id) { Tag = tag, File = file };

        var tagOnFileValue = new TagOnFileValue(tag.Id, file.Id, value) {
            TagOnFile = tagOnFile
        };

        // Act
        DbContext.Add(file);
        DbContext.Add(tag);
        DbContext.Add(tagOnFile);
        DbContext.Add(tagOnFileValue);
        await DbContext.SaveChangesAsync();

        // Assert
        tagOnFileValue.TagOnFile.Should().NotBeNull();
        tagOnFileValue.Value.Should().Be(value);

        // Verify the data was saved correctly in the database
        var savedValue = DbContext.Set<TagOnFileValue>().Single();
        savedValue.Value.Should().Be(value);
        savedValue.FileId.Should().Be(file.Id);
        savedValue.TagId.Should().Be(tag.Id);
    }
}
