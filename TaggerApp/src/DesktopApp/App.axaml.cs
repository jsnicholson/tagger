using System.IO;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using DesktopApp.ViewModels;
using DesktopApp.Views;

using Domain;
using Domain.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace DesktopApp;

public partial class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        // Hardcoded manifest path for now
        var manifestPath = new FileInfo("D:\\GitRepos\\tagger\\example-manifest\\tagger.db");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            var services = new ServiceCollection();
            services.AddTagDatabase();

            services.AddTransient<MainViewModel>();

            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IDatabaseManager>().ConnectToDatabase(manifestPath);

            var viewModel = provider.GetService<MainViewModel>();
            desktop.MainWindow = new MainWindow {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
