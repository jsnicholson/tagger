using Domain;
using Domain.Repositories;
using System.CommandLine;

namespace Cli.Commands;

public class ListFilesCommand : BaseCommand
{
    public ListFilesCommand(IDatabaseManager databaseManager)
        : base("list-files", "List all files in manifest", databaseManager) {
        this.SetHandler(ListFilesAsync, ManifestOption);
    }

    private async Task ListFilesAsync(FileInfo manifest) {
        using var context = OpenManifest(manifest);
        var fileRepository = new FileRepository(context);
        var files = await fileRepository.GetAllAsync();
        foreach (var file in files) {
            Console.WriteLine($"{file.id} | {file.path}");
        }
    }
}