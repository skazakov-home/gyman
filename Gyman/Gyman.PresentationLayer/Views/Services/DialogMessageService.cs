using System.Windows;

namespace Gyman.PresentationLayer.Views.Services
{
    public enum DialogMessageResult
    {
        OK,
        Cancel
    }

    public class DialogMessageService : IDialogMessageService
    {
        public DialogMessageResult ShowOkCancelDialog(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel);

            return result == MessageBoxResult.OK
                ? DialogMessageResult.OK
                : DialogMessageResult.Cancel;
        }

        public DialogMessageResult ShowInfoDialog(string message)
        {
            const string caption = "Info";

            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);

            return DialogMessageResult.OK;
        }
    }
}
