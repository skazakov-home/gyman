namespace Gyman.PresentationLayer.Views.Services
{
    public interface IDialogMessageService
    {
        DialogMessageResult ShowInfoDialog(string message);
        DialogMessageResult ShowOkCancelDialog(string message, string caption);
    }
}