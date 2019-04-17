using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AideDeJeu.Tools
{
    public class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        private bool ContinueOnCapturedContext = false;
        public NotifyTaskCompletion(Task<TResult> task, bool continueOnCapturedContext = false)
        {
            Task = task;
            ContinueOnCapturedContext = continueOnCapturedContext;
            if (task != null && !task.IsCompleted)
            {
                var _ = WatchTaskAsync(task, continueOnCapturedContext);
            }
        }
        private async Task WatchTaskAsync(Task task, bool continueOnCapturedContext = false)
        {
            try
            {
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch
            {
            }
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("DebugStatus"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }
        public Task<TResult> Task { get; private set; }
        public TResult Result
        {
            get
            {
                return (Task?.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult);
            }
        }
        public string DebugStatus { get { return Status.ToString(); } }
        public TaskStatus Status { get { return Task?.Status ?? TaskStatus.Running; } }
        public bool IsCompleted { get { return Task?.IsCompleted ?? false; } }
        public bool IsNotCompleted { get { return Task?.IsCompleted == true ? false : true; } }
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task?.Status == TaskStatus.RanToCompletion;
            }
        }
        public bool IsCanceled { get { return Task?.IsCanceled ?? false; } }
        public bool IsFaulted { get { return Task?.IsFaulted ?? false; } }
        public AggregateException Exception { get { return Task?.Exception; } }
        public Exception InnerException
        {
            get
            {
                return (Exception == null) ? null : Exception.InnerException;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return (InnerException == null) ? null : InnerException.Message;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}