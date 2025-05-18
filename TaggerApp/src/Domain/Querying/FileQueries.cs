using File = Domain.Entities.File;

namespace Domain.Querying;

public class PathContainsQuery(string substring, bool caseSensitive = false) : IEntityQuery<File> {
    public IQueryable<File> Apply(IQueryable<File> source) {
        return caseSensitive
            ? source.Where(f => f.Path.Contains(substring))
            : source.Where(f => f.Path.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
    }
}

public class TaggedByQuery(string tagName, bool caseSensitive = false) : IEntityQuery<File> {
    public IQueryable<File> Apply(IQueryable<File> source) {
        return caseSensitive
            ? source.Where(f => f.TaggedBy.Any(t => t.Name == tagName))
            : source.Where(f => f.TaggedBy.Any(t => t.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase)));
    }
}

public class TagValueEqualsQuery(string tagName, string value, bool caseSensitive = false) : IEntityQuery<File> {
    public IQueryable<File> Apply(IQueryable<File> source) {
        return caseSensitive
            ? source.Where(f => f.TagsOnFile.Any(tof =>
                tof.Tag.Name == tagName &&
                tof.Value == value))
            : source.Where(f => f.TagsOnFile.Any(tof =>
                tof.Tag.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase) &&
                (tof.Value != null && tof.Value.ToLower().Equals(value, StringComparison.CurrentCultureIgnoreCase))));
    }
}
