using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public abstract class Entity {
    // Optional per-entity config override
    public virtual void ConfigureEntity(ModelBuilder modelBuilder) { }
}
