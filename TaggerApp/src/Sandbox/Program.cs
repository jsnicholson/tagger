using Domain;
using Domain.Extensions;
using Domain.Library;

using Microsoft.EntityFrameworkCore;

namespace Sandbox;

class Program {
    static void Main() {
        var path = "C:\\Program Files\\word.jpg";
        Console.WriteLine(FileTypeMap.GetFileType(path));

        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        optionsBuilder.UseSqlite($"Data Source=C:\\src\\tagger.db");
        using var context = new DbContext(optionsBuilder.Options);
        Console.WriteLine(context.Database.GetDatabasePath());
    }
}
