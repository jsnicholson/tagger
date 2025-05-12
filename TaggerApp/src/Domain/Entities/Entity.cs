using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public abstract class Entity {
    // Optional per-entity config override
    public virtual void ConfigureEntity(ModelBuilder modelBuilder) { }
}