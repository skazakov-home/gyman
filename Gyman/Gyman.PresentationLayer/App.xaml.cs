using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Gyman.PresentationLayer.Startup;
using Gyman.PresentationLayer.Views;

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

            mainView.Show();
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
