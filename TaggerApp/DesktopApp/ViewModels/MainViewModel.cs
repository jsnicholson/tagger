using System.Collections.ObjectModel;

namespace DesktopApp.ViewModels;

public class MainViewModel : ViewModelBase {
    public string Greeting => "Welcome to Avalonia!";
    public ObservableCollection<Domain.Entities.File> Files { get; } = [];

    public MainViewModel() {
        int count = 20;
        for (int i = 0; i < count; i++) {
            Files.Add(new() { Path = $"content\\file{i}.txt"});
        }
    }
}
