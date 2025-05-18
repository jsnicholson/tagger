using Domain.Entities;

namespace Domain.Querying;

public class NameContainsQuery(string substring, bool caseSensitive = false) : IEntityQuery<Tag> {
    public IQueryable<Tag> Apply(IQueryable<Tag> source) {
        return caseSensitive
            ? source.Where(t => t.Name.Contains(substring))
            : source.Where(t => t.Name.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
    }
}

public class AppliesToTagQuery(string targetTagName, bool caseSensitive = false) : IEntityQuery<Tag> {
    public IQueryable<Tag> Apply(IQueryable<Tag> source)
    {
        return caseSensitive
            ? source.Where(t =>
                t.TagOnTags.Any(tot => tot.Target.Name.ToLower().Equals(targetTagName, StringComparison.CurrentCultureIgnoreCase)))
            : source.Where(t =>
                t.TagOnTags.Any(tot => tot.Target.Name == targetTagName));
    }
}
