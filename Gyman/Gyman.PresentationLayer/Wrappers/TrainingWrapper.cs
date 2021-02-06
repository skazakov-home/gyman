using System;
using Gyman.BusinessLogicLayer;

namespace Gyman.PresentationLayer.Wrappers
{
    public class TrainingWrapper : ModelWrapper<Training>
    {
        public TrainingWrapper(Training model)
            : base(model)
        { }

        public int Id => Model.Id;

        public DateTime Start
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime End
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int TrainerId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int MemberId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}
