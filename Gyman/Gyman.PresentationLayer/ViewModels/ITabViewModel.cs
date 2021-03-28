using System.Threading.Tasks;

namespace Gyman.PresentationLayer.ViewModels
{
    public interface ITabViewModel
    {
        int Id { get; }
        string Title { get; set; }
        Task LoadAsync(int id);
        bool HasChanges { get; }
    }
}
