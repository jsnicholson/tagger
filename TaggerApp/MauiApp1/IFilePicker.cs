namespace MauiApp1 {
    public interface IFilePicker {
        Task<string> PickSaveFileAsync(string suggestedFileName);
        Task<string> PickOpenFileAsync();
    }
}