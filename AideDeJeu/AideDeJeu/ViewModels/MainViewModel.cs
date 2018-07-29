using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private Dictionary<string, Item> _AllItems = new Dictionary<string, Item>();
        public async Task<Item> GetItemFromDataAsync(string source)
        {
            if (!_AllItems.ContainsKey(source))
            {
                //var md = await Tools.Helpers.GetStringFromUrl($"https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/{source}.md");
                var md = await Tools.Helpers.GetResourceStringAsync($"AideDeJeu.Data.{source}.md");
                //return Tools.MarkdownExtensions.ToItem(md);
                if (md != null)
                {
                    _AllItems[source] = Tools.MarkdownExtensions.ToItem(md);
                }
                else
                {
                    return null;
                }
            }
            return _AllItems[source];
        }

        public Command LoadItemsCommand { get; private set; }
        public Command AboutCommand { get; private set; }

        public Navigator Navigator { get; set; }

        public MainViewModel()
        {
            AboutCommand = new Command(async () => await Main.Navigator.GotoAboutPageAsync());
        }
    }
}