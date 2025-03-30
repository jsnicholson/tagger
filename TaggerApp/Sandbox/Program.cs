using Meta.Domain.Repositories;

namespace Sandbox;

class Program
{
    static void Main()
    {
        var repository = new FileSystemRepository("C:/Users/joshua.nicholson/Downloads");
        
        foreach(var file in repository.GetAllSubfolders(true))
            Console.WriteLine(file);
    }
}