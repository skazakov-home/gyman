using Autofac;
using Gyman.PresentationLayer.ViewModels;
using Gyman.PresentationLayer.Views;
using Gyman.PresentationLayer.Views.Services;

namespace Gyman.PresentationLayer.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<DialogMessageService>().As<IDialogMessageService>();

            return builder.Build();
        }
    }
}
