using System.Threading.Tasks;

namespace Gyman.PresentationLayer.Views.Services
{
    public interface IDialogMessageService
    {
        Task<DialogMessageResult> ShowInfoDialogAsync(string message);
        Task<DialogMessageResult> ShowOkCancelDialogAsync(string message, string caption);
    }
}