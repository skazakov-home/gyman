using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gyman.PresentationLayer.Events;
using Gyman.PresentationLayer.Views.Services;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator eventAggregator;
        protected readonly IDialogMessageService dialogMessageService;
        private bool hasChanges;
        private string title;

        public DetailViewModelBase(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService)
        {
            this.eventAggregator = eventAggregator;
            this.dialogMessageService = dialogMessageService;
            RegisterCommandHandlers();
        }

        public bool HasChanges
        {
            get => hasChanges;
            set
            {
                hasChanges = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public int Id { get; protected set; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CloseDetailViewCommand { get; private set; }

        public abstract Task LoadAsync(int id);

        private void RegisterCommandHandlers()
        {
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            DeleteCommand = new DelegateCommand(OnDelete);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailView);
        }

        protected abstract bool CanSave();

        protected abstract void OnSave();

        protected abstract void OnDelete();

        protected virtual async void OnCloseDetailView()
        {
            if (HasChanges)
            {
                var result = await dialogMessageService.ShowOkCancelDialogAsync(
                    "You've made changes. Close this item?", "Question");

                if (result == DialogMessageResult.Cancel)
                {
                    return;
                }
            }

            eventAggregator.GetEvent<DetailViewClosedEvent>().Publish(
                new DetailViewClosedEventArgs
                {
                    Id = Id,
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void RaiseDetailViewDeletedEvent(int modelId)
        {
            eventAggregator.GetEvent<DetailViewDeletedEvent>().Publish(
                new DetailViewDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = GetType().Name
                });
        }

        protected virtual void RaiseDetailViewSavedEvent(int modelId, string displayMember)
        {
            eventAggregator.GetEvent<DetailViewSavedEvent>().Publish(
                new DetailViewSavedEventArgs
                {
                    Id = modelId,
                    DisplayMember = displayMember,
                    ViewModelName = GetType().Name
                });
        }

        protected async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc, Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException e)
            {
                var databaseValues = e.Entries.Single().GetDatabaseValues();

                if (databaseValues == null)
                {
                    await dialogMessageService.ShowInfoDialogAsync(
                        "The entity has been deleted by another user.");
                    RaiseDetailViewDeletedEvent(Id);

                    return;
                }

                var result = await dialogMessageService.ShowOkCancelDialogAsync(
                    "The entity has been changed in the mean time by someone else. " +
                    "Click OK to save you changes anyway, click Cancel to reload " +
                    "the entity from the database.", "Question");

                if (result == DialogMessageResult.OK)
                {
                    var entry = e.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    await e.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            }

            afterSaveAction();
        }
    }
}
