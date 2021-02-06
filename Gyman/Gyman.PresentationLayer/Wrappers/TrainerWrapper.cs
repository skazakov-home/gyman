using Gyman.BusinessLogicLayer;

namespace Gyman.PresentationLayer.Wrappers
{
    public class TrainerWrapper : ModelWrapper<Trainer>
    {
        public TrainerWrapper(Trainer model)
            : base(model)
        { }

        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Surname
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Phone
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsBusy
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }
}
