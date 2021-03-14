using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Gyman.PresentationLayer.Views.Services
{
    public enum DialogMessageResult
    {
        OK,
        Cancel
    }

    public class DialogMessageService : IDialogMessageService
    {
        private MetroWindow MainWindow => (MetroWindow)Application.Current.MainWindow;

        public async Task<DialogMessageResult> ShowOkCancelDialogAsync(string message, string caption)
        {
            var result = await MainWindow
                .ShowMessageAsync(caption, message, MessageDialogStyle.AffirmativeAndNegative);

            return result == MessageDialogResult.Affirmative
                ? DialogMessageResult.OK
                : DialogMessageResult.Cancel;
        }

        public async Task<DialogMessageResult> ShowInfoDialogAsync(string message)
        {
            const string caption = "Info";
            await MainWindow.ShowMessageAsync(caption, message);

            return DialogMessageResult.OK;
        }
    }
}
