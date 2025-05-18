namespace Domain.Querying;

public class AndQuery<T>(IEntityQuery<T> left, IEntityQuery<T> right) : IEntityQuery<T> {
    public IQueryable<T> Apply(IQueryable<T> source) {
        return right.Apply(left.Apply(source));
    }
}

public class OrQuery<T>(IEntityQuery<T> left, IEntityQuery<T> right) : IEntityQuery<T> {
    public IQueryable<T> Apply(IQueryable<T> source) {
        var leftResult = left.Apply(source);
        var rightResult = right.Apply(source);
        return leftResult.Union(rightResult);
    }
}

public class NotQuery<T>(IEntityQuery<T> inner) : IEntityQuery<T> {
    public IQueryable<T> Apply(IQueryable<T> source) {
        var excluded = inner.Apply(source);
        return source.Except(excluded);
    }
}

public class XorQuery<T>(IEntityQuery<T> left, IEntityQuery<T> right) : IEntityQuery<T> {
    public IQueryable<T> Apply(IQueryable<T> source) {
        var leftResult = left.Apply(source);
        var rightResult = right.Apply(source);
        var both = leftResult.Intersect(rightResult);

        return leftResult.Union(rightResult).Except(both);
    }
}
