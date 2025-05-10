using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddTagDatabase(this IServiceCollection services) {
        services.AddSingleton<IDatabaseManager, DatabaseManager>();
        services.AddSingleton<TagDbContextFactory>();
        services.AddScoped<IFileRepository>(sp => {
            var factory = sp.GetRequiredService<TagDbContextFactory>();
            return new FileRepository(factory.CreateDbContext());
        });
        services.AddScoped<ITagRepository>(sp => {
            var factory = sp.GetRequiredService<TagDbContextFactory>();
            return new TagRepository(factory.CreateDbContext());
        });
        services.AddScoped<ITagOnFileRepository>(sp => {
            var factory = sp.GetRequiredService<TagDbContextFactory>();
            return new TagOnFileRepository(factory.CreateDbContext());
        });

        return services;
    }
}