using Domain.Library;

namespace Sandbox;

class Program
{
    static void Main()
    {
        var path = "C:\\Program Files\\word.jpg";
        Console.WriteLine(FileTypeMap.GetFileType(path));
    }
}