using Windows.Storage.Pickers;

namespace MauiApp1.Platforms.Windows {
    public class FilePickerImplementation : IFilePicker {
        public async Task<string> PickSaveFileAsync(string suggestedFileName) {
            var savePicker = new FileSavePicker {
                SuggestedStartLocation = PickerLocationId.Desktop,
                SuggestedFileName = suggestedFileName
            };
            savePicker.FileTypeChoices.Add("SQLite Database", new List<string>() { ".db" });

            var hwnd = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            var file = await savePicker.PickSaveFileAsync();
            return file?.Path;
        }

        public async Task<string> PickOpenFileAsync() {
            var openPicker = new FileOpenPicker {
                SuggestedStartLocation = PickerLocationId.Desktop,
                FileTypeFilter = { ".db" }
            };

            var hwnd = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            var file = await openPicker.PickSingleFileAsync();
            return file?.Path;
        }
    }
}