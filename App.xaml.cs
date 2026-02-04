using System;
using System.IO;
using System.Windows;
using NexusPOS.Data.Context;
using NexusPOS.ViewModels;
using NexusPOS.Views.Windows;

namespace NexusPOS
{
    public partial class App : Application
    {
        private const string LogFileName = "error_log.txt";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup Global Exception Handler
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Initialize Database
            using (var context = new AppDbContext())
            {
                // Creates the DB if not exists and applies migrations/seed data
                context.Database.EnsureCreated(); 
            }
            
            // Create and show the main window
            var mainViewModel = new MainViewModel();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Prevent default unhandled exception processing
            e.Handled = true;
            var errorMessage = $"An unexpected error occurred: {e.Exception.Message}\n\n{e.Exception.StackTrace}";
            File.AppendAllText(LogFileName, $"{DateTime.Now}: {errorMessage}\n\n");
            MessageBox.Show("An unexpected error occurred. The application will now close. Please check the error_log.txt file for details.", "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Current.Shutdown();
        }
    }
}
