using AideDeJeuLib;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class SpeechViewModel : BaseViewModel
    {
        private Command<Item> _SpeakItemCommand = null;
        public Command<Item> SpeakItemCommand
        {
            get
            {
                return _SpeakItemCommand ?? (_SpeakItemCommand = new Command<Item>(async (item) => await ExecuteSpeakItemCommandAsync(item)));
            }
        }

        private Command _CancelSpeakCommand = null;
        public Command CancelSpeakCommand
        {
            get
            {
                return _CancelSpeakCommand ?? (_CancelSpeakCommand = new Command(() => ExecuteCancelSpeakCommand()));
            }
        }

        public bool Speaking
        {
            get
            {
                return _CancellationTokenSource != null;
            }
        }
        public bool NotSpeaking
        {
            get
            {
                return _CancellationTokenSource == null;
            }
        }
        private CancellationTokenSource _CancellationTokenSource = null;
        public async Task ExecuteSpeakItemCommandAsync(Item item)
        {
            if(Speaking)
            {
                ExecuteCancelSpeakCommand();
                return;
            }
            var md = item.Markdown;
            try
            {
                _CancellationTokenSource = new CancellationTokenSource();
                OnPropertyChanged(nameof(Speaking));
                OnPropertyChanged(nameof(NotSpeaking));
                await Xamarin.Essentials.TextToSpeech.SpeakAsync(md, _CancellationTokenSource.Token);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                _CancellationTokenSource?.Dispose();
                _CancellationTokenSource = null;
                OnPropertyChanged(nameof(Speaking));
                OnPropertyChanged(nameof(NotSpeaking));
            }
        }

        public void ExecuteCancelSpeakCommand()
        {
            _CancellationTokenSource?.Cancel();
        }
    }
}
