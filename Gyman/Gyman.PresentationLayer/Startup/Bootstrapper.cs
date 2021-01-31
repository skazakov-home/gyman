using Autofac;
using Gyman.PresentationLayer.ViewModels;
using Gyman.PresentationLayer.Views;

namespace Gyman.PresentationLayer.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            return builder.Build();
        }
    }
}
