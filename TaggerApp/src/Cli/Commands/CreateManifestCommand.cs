using System.CommandLine;
using Cli.Commands;
using Domain;

namespace CLI.Commands;

public class CreateManifestCommand : BaseCommand
{
    private readonly DatabaseManager _databaseManager;

    public static readonly Option<bool> OverwriteOption = new(
        ["-o", "--overwrite"],
        "Overwrite the manifest if it already exists"
    );

    public CreateManifestCommand(DatabaseManager databaseManager)
        : base("create", "Create a new manifest")
    {
        _databaseManager = databaseManager;

        AddOption(OverwriteOption);

        this.SetHandler(CreateManifestAsync, ManifestOption, OverwriteOption);
    }

    private async Task CreateManifestAsync(FileInfo manifest, bool overwrite)
    {
        await _databaseManager.InitialiseDatabaseAsync(manifest, overwrite);
    }
}