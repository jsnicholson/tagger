namespace Domain.Querying;

public interface IEntityQuery<T> {
    IQueryable<T> Apply(IQueryable<T> source);
}
