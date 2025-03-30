using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CLI.Commands;
using Domain;

namespace CLI;

internal static class Program
{
    static async Task<int> Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<DatabaseManager>();
                services.AddSingleton<CreateVaultCommand>();
                services.AddDbContext<TagDbContext>();
            })
            .Build();

        var rootCommand = new RootCommand
        {
            Name = "tagger",
        };
        rootCommand.AddCommand(host.Services.GetRequiredService<CreateVaultCommand>());
        
        return rootCommand.InvokeAsync(args).Result;
    }
}