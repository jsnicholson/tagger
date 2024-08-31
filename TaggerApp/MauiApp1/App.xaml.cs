namespace MauiApp1 {
    public partial class App : Application {
        public App(IServiceProvider serviceProvider) {
            InitializeComponent();

            MainPage = serviceProvider.GetRequiredService<MainPage>();
        }
    }
}
