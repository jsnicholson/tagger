using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

using Avalonia.Controls;

using Domain.Repositories;

using DynamicData;

using ReactiveUI;

namespace DesktopApp.ViewModels;

public class MainViewModel : ViewModelBase {
    private readonly IFileRepository _fileRepository;
    private List<Domain.Entities.File> _files = [];
    public List<Domain.Entities.File> Files {
        get => _files;
        set {
            _files = value;
            UpdateFileItems();
        }
    }
    public ObservableCollection<FileItemViewModel> FileItems = [];
    public ReactiveCommand<Unit, Unit> LoadFilesCommand { get; }
    private FileItemViewModel? _selectedFile;

    public FileItemViewModel? SelectedFile {
        get => _selectedFile;
        set => this.RaiseAndSetIfChanged(ref _selectedFile, value);
    }

    public string? SelectedFileSize => SelectedFile != null
        ? new FileInfo(SelectedFile.Path).Length.ToString()
        : null;

    public string? SelectedFileModified => SelectedFile != null
        ? File.GetLastWriteTime(SelectedFile.Path).ToString()
        : null;

    // Design-time constructor
    public MainViewModel() {
        if (Design.IsDesignMode) {
            // Add dummy data for previewing
            Files.AddRange([
                new("cat.jpg"),
                new("document.docx"),
                new("spreadsheet.xslx"),
                new("notes.txt"),
                new("video.mp4")
                ]);
            UpdateFileItems();
        }
    }

    public MainViewModel(IFileRepository fileRepository) {
        _fileRepository = fileRepository;

        LoadFilesCommand = ReactiveCommand.CreateFromTask(LoadFilesAsync);
        LoadFilesCommand.Execute().Subscribe(); // Load files on init
    }

    private async Task LoadFilesAsync() {
        var files = await _fileRepository.GetAllAsync();
        Files.Clear();
        Files.AddRange(files);
        UpdateFileItems();
    }

    private void UpdateFileItems() {
        var newFileIds = new HashSet<Guid>(_files.Select(f => f.Id));
        var existingFileIds = new HashSet<Guid>(FileItems.Select(vm => vm.Id));

        // Remove items no longer present
        var toRemove = FileItems.Where(vm => !newFileIds.Contains(vm.Id)).ToList();
        foreach (var vm in toRemove) {
            FileItems.Remove(vm);
        }

        // Add new items
        var existingIds = new HashSet<Guid>(FileItems.Select(vm => vm.Id));
        foreach (var file in _files) {
            if (!existingIds.Contains(file.Id)) {
                FileItems.Add(new FileItemViewModel(file));
            }
        }

        // Optionally: update existing items in-place if name/path/etc. can change
        foreach (var file in _files) {
            var existing = FileItems.FirstOrDefault(vm => vm.Id == file.Id);
            if (existing != null) {
                existing.Path = file.Path; // Will need to raise PropertyChanged in FileItemViewModel
            }
        }
    }
}
