using Domain.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.CommandLine;

namespace Cli.Commands {
    class ListTagsCommand : BaseCommand {
        public ListTagsCommand()
        : base("list-tags", "List all tags in manifest") {

            this.SetHandler(ListTagsAsync, ManifestOption);
        }

        private async Task ListTagsAsync(FileInfo manifest) {
            var optionsBuilder = new DbContextOptionsBuilder<TagDbContext>();
            optionsBuilder.UseSqlite($"Data Source={manifest.FullName}");
            using var context = new TagDbContext(optionsBuilder.Options);
            var tagRepository = new TagRepository(context);
            var tags = await tagRepository.GetAllAsync();
            foreach (var tag in tags) {
                Console.WriteLine($"{tag.Id} | {tag.Name}");
            }
        }
    }
}