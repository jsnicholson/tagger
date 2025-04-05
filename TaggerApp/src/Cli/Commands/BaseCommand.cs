using Domain;
using System.CommandLine;

namespace Cli.Commands;

public class BaseCommand : Command {
    protected readonly IDatabaseManager _databaseManager;
    public static readonly Option<FileInfo> ManifestOption = new(
        [ "--manifest", "-m" ],
        "Path to the manifest file"
    ) {
        IsRequired = true,
    };

    protected BaseCommand(string name, string description, IDatabaseManager databaseManager) : base(name, description) {
        _databaseManager = databaseManager;
        AddOption(ManifestOption);
    }

    protected TagDbContext OpenManifest(FileInfo manifest) {
        return _databaseManager.ConnectToDatabase(manifest);
    }
}
