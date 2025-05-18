using AutoFixture;
using AutoFixture.NUnit3;
using Domain.Entities;
using File = Domain.Entities.File;

namespace Domain.Test;

public class CustomAutoDataAttribute() : AutoDataAttribute(() => {
    var fixture = new Fixture();
    fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    fixture.Customize(new EntityCustomization());
    return fixture;
});

public class EntityCustomization : ICustomization {
    public void Customize(IFixture fixture) {
        // Prevent deep graph creation
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        // Ignore navigation properties explicitly
        fixture.Customize<File>(c => c
            .Without(f => f.TagsOnFile));

        fixture.Customize<Tag>(c => c
            .Without(t => t.TagsOnTag)
            .Without(t => t.TagOnTags)
            .Without(t => t.TagOnFiles));

        fixture.Customize<TagOnFile>(c => c
            .Without(tof => tof.Tag)
            .Without(tof => tof.File));

        fixture.Customize<TagOnTag>(c => c
            .Without(tot => tot.Source)
            .Without(tot => tot.Target));
    }
}
