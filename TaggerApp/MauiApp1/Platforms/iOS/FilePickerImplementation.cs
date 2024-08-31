namespace MauiApp1.Platforms.iOS {
    public class FilePickerImplementation : IFilePicker {
        public Task<string> PickSaveFileAsync(string suggestedFileName) {
            var savePanel = new NSSavePanel {
                Title = "Save SQLite Database",
                AllowedFileTypes = new[] { "db" },
                NameFieldStringValue = suggestedFileName
            };

            if (savePanel.RunModal() == 1) {
                return Task.FromResult(savePanel.Url.Path);
            }

            return Task.FromResult<string>(null);
        }
    }
}