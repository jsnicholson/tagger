using System.CommandLine;
using System.CommandLine.Invocation;
using Domain;

namespace CLI.Commands;

public class CreateVaultCommand : Command
{
    private readonly DatabaseManager _databaseManager;
    
    public CreateVaultCommand(DatabaseManager databaseManager)
        : base("create-vault", "Create a new vault")
    {
        _databaseManager = databaseManager;
        
        /*var nameOption = new Option<string>(
            "--name",
            "The name of the vault to create")
        {
            IsRequired = true
        };
        AddOption(nameOption);*/
        
        var pathOption = new Option<string>(
            "--path",
            "Where to create the vault")
        {
            IsRequired = true
        };
        AddOption(pathOption);
        
        this.SetHandler(async (path) =>
        {
            await CreateVaultAsync(path);
        }, pathOption);
    }

    private static async Task CreateVaultAsync(string path)
    {
        Console.WriteLine("creating");
        await DatabaseManager.InitialiseDatabaseAsync(path);
        
        Console.WriteLine("Created vault (not really)");
    }
}