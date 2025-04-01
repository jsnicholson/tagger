using System.CommandLine;
using System.CommandLine.Invocation;

namespace Cli.Commands
{
    public class BaseCommand : Command {
        public static readonly Option<FileInfo> ManifestOption = new(
            [ "--manifest", "-m" ],
            "Path to the manifest file"
        ) {
            IsRequired = true,
        };

        protected BaseCommand(string name, string description) : base(name, description) {
            AddOption(ManifestOption);

            this.SetHandler((FileInfo manifest) =>
            {
                if (!Path.IsPathRooted(manifest.FullName)) {
                    manifest = new FileInfo(Path.GetFullPath(manifest.FullName));
                }

                return Task.CompletedTask;
            }, ManifestOption);
        }
    }
}
