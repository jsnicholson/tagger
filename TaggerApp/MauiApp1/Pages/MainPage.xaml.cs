using Core;
using MauiApp1.Pages;
using System.Windows;
using Windows.Storage.Pickers;

namespace MauiApp1 {
    public partial class MainPage : ContentPage {
        private readonly IFilePicker _filePicker;
        int count = 0;

        public MainPage(IFilePicker fileSavePicker) {
            _filePicker = fileSavePicker;
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e) {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnCreateLibraryClicked(object sender, EventArgs e) {
            string dbFilePath = await _filePicker.PickSaveFileAsync("NewDatabase.db");

            if (!string.IsNullOrEmpty(dbFilePath)) {
                try {
                    using (var context = DatabaseManager.CreateDbContext(dbFilePath)) {
                        await DisplayAlert("Success", $"Database created at {dbFilePath}", "OK");
                    }
                } catch (Exception ex) {
                    await DisplayAlert("Error", $"Failed to create database: {ex.Message}", "OK");
                }
            }
        }

        private async void OnOpenLibraryClicked(object sender, EventArgs e) {
            string dbPath = await _filePicker.PickOpenFileAsync();

            if (dbPath != null) {
                await Navigation.PushAsync(new PageWithContext());
            }
        }
    }
}