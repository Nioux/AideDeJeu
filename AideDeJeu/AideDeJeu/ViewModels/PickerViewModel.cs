using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class PickerViewModel<T> : BaseViewModel where T:class
    {
        private string _Title = null;
        public new string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                SetProperty(ref _Title, value);
            }
        }

        private System.Collections.IEnumerable _Items = null;
        public System.Collections.IEnumerable Items
        {
            get
            {
                return _Items;
            }
            set
            {
                SetProperty(ref _Items, value);
            }
        }

        private T _SelectedItem = null;
        public T SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                SetProperty(ref _SelectedItem, value);
                if (_taskCompletionSource != null)
                {
                    _taskCompletionSource.SetResult(value);
                    _taskCompletionSource = null;
                }
            }
        }
        private TaskCompletionSource<T> _taskCompletionSource;
        public Task<T> PickValueAsync()
        {
            _taskCompletionSource = new TaskCompletionSource<T>();
            return _taskCompletionSource.Task;
        }
    }
}
