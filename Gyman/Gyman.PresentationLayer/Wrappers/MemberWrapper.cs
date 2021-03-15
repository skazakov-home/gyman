﻿using System.Collections.Generic;
using Gyman.BusinessLogicLayer;

namespace Gyman.PresentationLayer.Wrappers
{
    public class MemberWrapper : ModelWrapper<Member>
    {
        public MemberWrapper(Member model)
            : base(model)
        { }

        public int Id => Model.Id;

        public bool HasSubscription => Model.Subscription != null;

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

        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Phone
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Age
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public double Weight
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public double Height
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public int? SubscriptionId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Age):
                    if (AgeIsNotValid())
                    {
                        yield return "Age must be between 1 and 120.";
                    }
                    break;
                case nameof(Weight):
                    if (WeightIsNotValid())
                    {
                        yield return "Weight cannot be negative or zero.";
                    }
                    break;
                case nameof(Height):
                    if (HeightIsNotValid())
                    {
                        yield return "Height cannot be negative or zero.";
                    }
                    break;
            }
        }

        private bool AgeIsNotValid()
        {
            return Age <= 0 || Age > 120;
        }

        private bool WeightIsNotValid()
        {
            return Weight <= 0;
        }

        private bool HeightIsNotValid()
        {
            return Height <= 0;
        }
    }
}
