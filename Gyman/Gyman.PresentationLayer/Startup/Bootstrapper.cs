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
            builder.RegisterType<MemberDetailViewModel>().Keyed<ITabViewModel>(nameof(MemberDetailViewModel));
            builder.RegisterType<TrainerDetailViewModel>().Keyed<ITabViewModel>(nameof(TrainerDetailViewModel));
            builder.RegisterType<TrainingDetailViewModel>().Keyed<ITabViewModel>(nameof(TrainingDetailViewModel));
            builder.RegisterType<SubscriptionDetailViewModel>().Keyed<ITabViewModel>(nameof(SubscriptionDetailViewModel));
            builder.RegisterType<MembersViewModel>().Keyed<ITabViewModel>(nameof(MembersViewModel));
            builder.RegisterType<TrainersViewModel>().Keyed<ITabViewModel>(nameof(TrainersViewModel));

            builder.RegisterType<GymanDbContext>().AsSelf();

            builder.RegisterType<DialogMessageService>().As<IDialogMessageService>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            builder.RegisterType<MemberRepository>().As<IMemberRepository>();
            builder.RegisterType<TrainerRepository>().As<ITrainerRepository>();
            builder.RegisterType<TrainingRepository>().As<ITrainingRepository>();
            builder.RegisterType<SubscriptionRepository>().As<ISubscriptionRepository>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            return builder.Build();
        }
    }
}
