﻿using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Cli.Commands;
using Domain;
using Domain.Repositories;

namespace Cli;

internal static class Program
{
    static async Task<int> Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
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
                services.AddSingleton<IDatabaseManager, DatabaseManager>();
                services.AddSingleton<CreateManifestCommand>();
                services.AddSingleton<ListFilesCommand>();
                services.AddSingleton<ListTagsCommand>();
                services.AddDbContext<TagDbContext>();
            })
            .Build();

        var rootCommand = new RootCommand
        {
            Name = "tagger",
        };
        rootCommand.AddCommand(host.Services.GetRequiredService<CreateManifestCommand>());
        rootCommand.AddCommand(host.Services.GetRequiredService<ListFilesCommand>());
        rootCommand.AddCommand(host.Services.GetRequiredService<ListTagsCommand>());
        
        return await rootCommand.InvokeAsync(args);
    }
}