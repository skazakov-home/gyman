using System.Threading.Tasks;

namespace Gyman.PresentationLayer.ViewModels
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int id);
        bool HasChanges { get; }
        int Id { get; }
    }
}