using System.CommandLine;
using Cli.Commands;
using Domain;
using Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cli;

internal static class Program {
    static async Task<int> Main(string[] args) {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.AddTagDatabase();
                services.AddSingleton<CreateManifestCommand>();
                services.AddSingleton<ListFilesCommand>();
                services.AddSingleton<ListTagsCommand>();
                services.AddDbContext<TagDbContext>();
            })
            .Build();

        var rootCommand = new RootCommand {
            Name = "tagger",
        };
        rootCommand.AddCommand(host.Services.GetRequiredService<CreateManifestCommand>());
        rootCommand.AddCommand(host.Services.GetRequiredService<ListFilesCommand>());
        rootCommand.AddCommand(host.Services.GetRequiredService<ListTagsCommand>());

        return await rootCommand.InvokeAsync(args);
    }
}
