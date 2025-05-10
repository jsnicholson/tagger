using Avalonia.Controls;
using ReactiveUI;
using System;

namespace DesktopApp.ViewModels;

public class FileItemViewModel : ReactiveObject {
    private Guid _id;
    private string _path;

    public Guid Id {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string Path {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }

    // Design-time constructor
    public FileItemViewModel() {
        if (Design.IsDesignMode) {
            Id = Guid.NewGuid();
            Path = "C://Documents/notes.txt";
        }
    }

    public FileItemViewModel(Domain.Entities.File file) {
        Id = file.Id;
        Path = file.Path;
    }
}