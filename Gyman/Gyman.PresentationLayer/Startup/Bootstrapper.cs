using Autofac;
using Gyman.DataAccessLayer;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Data.Services;
using Gyman.PresentationLayer.ViewModels;
using Gyman.PresentationLayer.Views;
using Gyman.PresentationLayer.Views.Services;
using Prism.Events;

namespace Gyman.PresentationLayer.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<MemberDetailViewModel>().Keyed<IDetailViewModel>(nameof(MemberDetailViewModel));
            builder.RegisterType<GymanDbContext>().AsSelf();
            builder.RegisterType<DialogMessageService>().As<IDialogMessageService>();
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<MemberRepository>().As<IMemberRepository>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            return builder.Build();
        }
    }
}
