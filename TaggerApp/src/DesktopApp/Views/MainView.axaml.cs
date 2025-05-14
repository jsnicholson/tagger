using Avalonia.Controls;
using Avalonia.Input;

using DesktopApp.ViewModels;

namespace DesktopApp.Views;

public partial class MainView : UserControl {
    public MainView() {
        InitializeComponent();
    }

    private void FileItem_PointerPressed(object? sender, PointerPressedEventArgs e) {
        if (sender is Control control && control.DataContext is FileItemViewModel vm) {
            if (DataContext is MainViewModel mainVM) {
                mainVM.SelectedFile = vm;
            }
        }
    }
}
