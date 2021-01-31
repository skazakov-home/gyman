using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Gyman.PresentationLayer.ViewModels;

namespace Gyman.PresentationLayer.Wrappers
{
    public class NotifyDataErrorInfoBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> errors =
            new Dictionary<string, List<string>>();

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return errors.ContainsKey(propertyName)
                ? errors[propertyName]
                : null;
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            base.OnPropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors[propertyName] = new List<string>();
            }

            if (!errors[propertyName].Contains(error))
            {
                errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
