using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Gyman.PresentationLayer.Startup;
using Gyman.PresentationLayer.Views;
using MahApps.Metro.Controls.Dialogs;

namespace Gyman.PresentationLayer
{
    public partial class App : Application
    {
        public App()
        {
            SubscribeToEvents();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var mainView = container.Resolve<MainView>();
            ShowLoginDialogAsync(mainView);
            mainView.Show();
        }

        private async void ShowLoginDialogAsync(MainView mainView)
        {
            const string title = "Login";
            const string message = "Administrator";

            var settings = new LoginDialogSettings
            {
                UsernameWatermark = "Login...",
                PasswordWatermark = "Password...",
                NegativeButtonVisibility = Visibility.Visible,
                EnablePasswordPreview = true
            };

            bool isDone;
            do
            {
                var result = await mainView.ShowLoginAsync(title, message, settings);

                if (result == null)
                {
                    Shutdown();
                    break;
                }

                isDone = result.Username == "Admin" && result.Password == "12345";
            } while (!isDone);
        }

        private void SubscribeToEvents()
        {
            DispatcherUnhandledException += OnApplicationDispatcherUnhandledException;
        }

        private void OnApplicationDispatcherUnhandledException(
            object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var caption = "Unexpected error";
            var message = "Unexpected error occured. Please inform the admin."
                + Environment.NewLine + e.Exception.Message;

            MessageBox.Show(message, caption);
        }
    }
}
