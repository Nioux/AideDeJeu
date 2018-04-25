using AideDeJeu.Tools;
using AideDeJeuLib.Spells;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reactive.Linq;

namespace AideDeJeu.ViewModels
{
    public class SpellDetailViewModel : BaseViewModel
    {
        Spell _Item = null;
        public Spell Item
        {
            get { return _Item; }
            set
            {
                SetProperty(ref _Item, value);
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(TypeLevel));
                OnPropertyChanged(nameof(CastingTime));
                OnPropertyChanged(nameof(Range));
                OnPropertyChanged(nameof(Components));
                OnPropertyChanged(nameof(Duration));
            }
        }

        public FormattedString Description
        {
            get
            {
                var fs = new FormattedString();
                if (Item?.DescriptionDiv != null)
                {
                    FormatedTextHelpers.HtmlToFormatedString(Item?.DescriptionDiv, fs, FontAttributes.None);
                }
                return fs;
            }
        }

        public FormattedString TypeLevel
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("contentital");
                var fs = new FormattedString();
                var capType = Item.Type.First().ToString().ToUpper() + Item.Type.Substring(1);
                fs.Spans.Add(new Span() { Text = string.Format("{0} de niveau {1}", capType, Item.Level), FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor});
                return fs;
            }
        }

        public FormattedString CastingTime
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Durée d'incantation : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.CastingTime, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString Range
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Portée : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.Range, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString Components
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Composantes : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.Components, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString Duration
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Durée : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.Duration, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public Command LoadItemCommand { get; set; }

        public SpellDetailViewModel(Spell item = null)
        {
            Title = item?.Title;
            Item = item;
            LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
        }
        async Task ExecuteLoadItemCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //Items.Clear();
                var item = await new Scrappers().GetSpell(Item.Id);
                await Akavache.BlobCache.UserAccount.Flush();
                Item = item;
                //foreach (var item in items)
                //{
                //    Items.Add(item);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }




}
