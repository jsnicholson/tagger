using Cli.Commands;
using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.CommandLine;

namespace CLI.Commands;

public class ListFilesCommand : BaseCommand
{
    public ListFilesCommand()
        : base("list-files", "List all files in manifest") {

        this.SetHandler(ListFilesAsync, ManifestOption);
    }

    private async Task ListFilesAsync(FileInfo manifest) {
        var optionsBuilder = new DbContextOptionsBuilder<TagDbContext>();
        optionsBuilder.UseSqlite($"Data Source={manifest.FullName}");
        using var context = new TagDbContext(optionsBuilder.Options);
        var fileRepository = new FileRepository(context);
        var files = await fileRepository.GetAllAsync();
        foreach (var file in files) {
            Console.WriteLine($"{file.id} | {file.path}");
        }
    }
}