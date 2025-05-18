using Domain.Entities;
using FluentAssertions;

namespace Domain.Test.Entities;

public class TagOnTagTests : BaseTest {
    [Test]
    public void CanCreateTagOnTag() {
        // Arrange
        var tagger = new Tag("tagger");
        var tagged = new Tag("tagged");
        var tagOnTag = new TagOnTag(tagger.Id, tagged.Id) {
            Source = tagger,
            Target = tagged
        };

        // Act
        DbContext.Add(tagger);
        DbContext.Add(tagged);
        DbContext.Add(tagOnTag);
        DbContext.SaveChanges();

        // Assert
        tagOnTag.Source.Should().NotBeNull();
        tagOnTag.Target.Should().NotBeNull();

        // Verify the data was saved correctly in the database
        var savedTagOnTag = DbContext.Set<TagOnTag>().Single();
        savedTagOnTag.TaggerId.Should().Be(tagger.Id);
        savedTagOnTag.TaggedId.Should().Be(tagged.Id);
    }
}
