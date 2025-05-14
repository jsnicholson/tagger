using System.CommandLine;
using System.CommandLine.Completions;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cli.Commands;

class ListTagsCommand : BaseCommand {
    public static readonly Option<string?> FileOption = new(
        ["-f", "--on-file"],
        "File on which to list tags"
    );

    public ListTagsCommand(IDatabaseManager databaseManager)
    : base("list-tags", "List all tags in manifest", databaseManager) {
        FileOption.AddCompletions(GetFileCompletions);
        AddOption(FileOption);

        this.SetHandler(ListTagsAsync, ManifestOption, FileOption);
    }

    private async Task ListTagsAsync(FileInfo manifest, string? fileInput) {
        using var context = OpenManifest(manifest);
        var tagRepository = new TagRepository(context);

        IEnumerable<Tag> tags;
        if (string.IsNullOrEmpty(fileInput)) {
            tags = await tagRepository.GetAllAsync();
        }
        else {
            // Try to match by ID or file name
            var file = await context.Files
                .FirstOrDefaultAsync(f => f.Id.ToString() == fileInput || f.Path.Contains(fileInput));

            if (file == null) {
                Console.WriteLine($"No file found matching '{fileInput}'");
                return;
            }

            tags = await tagRepository.GetTagsForFileAsync(file.Id);
        }

        if (!tags.Any()) {
            Console.WriteLine($"No tags found for file");
        }

        foreach (var tag in tags) {
            Console.WriteLine($"{tag.Id} | {tag.Name}");
        }
    }

    private IEnumerable<string> GetFileCompletions(CompletionContext context) {
        var manifestArg = context.ParseResult.GetValueForOption(ManifestOption);
        if (manifestArg == null) {
            return Enumerable.Empty<string>();
        }

        using var dbContext = OpenManifest(manifestArg);
        var files = dbContext.Files
            .Select(f => f.Id.ToString())
            .Concat(dbContext.Files.Select(f => f.Path))
            .ToList();
        return files;
    }
}
