using System.CommandLine;

using Domain;

namespace Cli.Commands;

public class CreateManifestCommand : BaseCommand {
    public static readonly Option<bool> OverwriteOption = new(
        ["-o", "--overwrite"],
        "Overwrite the manifest if it already exists"
    );

    public CreateManifestCommand(IDatabaseManager databaseManager)
        : base("create", "Create a new manifest", databaseManager) {
        AddOption(OverwriteOption);
        this.SetHandler(CreateManifestAsync, ManifestOption, OverwriteOption);
    }

    private async Task CreateManifestAsync(FileInfo manifest, bool overwrite) {
        await base._databaseManager.InitialiseDatabaseAsync(manifest, overwrite);
    }
}
