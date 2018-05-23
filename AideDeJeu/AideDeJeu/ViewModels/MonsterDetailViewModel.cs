using AideDeJeu.Tools;
using AideDeJeuLib.Monsters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class MonsterDetailViewModel : BaseViewModel
    {
        Monster _Item = null;
        public Monster Item
        {
            get { return _Item; }
            set
            {
                SetProperty(ref _Item, value);
                OnPropertyChanged(nameof(TypeSizeAlignment));
                OnPropertyChanged(nameof(ArmorClass));
                OnPropertyChanged(nameof(HitPoints));
                OnPropertyChanged(nameof(Speed));
                OnPropertyChanged(nameof(SavingThrows));
                OnPropertyChanged(nameof(Skills));
                OnPropertyChanged(nameof(DamageVulnerabilities));
                OnPropertyChanged(nameof(DamageResistances));
                OnPropertyChanged(nameof(DamageImmunities));
                OnPropertyChanged(nameof(ConditionImmunities));
                OnPropertyChanged(nameof(Senses));
                OnPropertyChanged(nameof(Languages));
                OnPropertyChanged(nameof(Challenge));
                //OnPropertyChanged(nameof(Duration));
                //OnPropertyChanged(nameof(Duration));
            }
        }

        public FormattedString TypeSizeAlignment
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("contentital");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = string.Format("{0} de taille {1}, {2}", Item.Type, Item.Size, Item.Alignment), FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString ArmorClass
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = string.Format("Classe d'armure {0}", Item.ArmorClass), FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString HitPoints
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = string.Format("Points de vie {0}", Item.HitPoints), FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString Speed
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = string.Format("Vitesse {0}", Item.Speed), FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString SavingThrows
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Jets de sauvegarde ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.SavingThrows, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString Skills
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Compétence ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.Skills, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }


        public FormattedString DamageVulnerabilities
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Vulnérabilité aux dégâts ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.DamageVulnerabilities, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString DamageImmunities
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Immunité contre les dégâts ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.DamageImmunities, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString ConditionImmunities
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Immunité contre les états ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.ConditionImmunities, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString DamageResistances
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Résistance aux dégâts ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.DamageResistances, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }



        public FormattedString Senses
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Sens ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.Senses, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString Languages
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Langues ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.Languages, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }

        public FormattedString Challenge
        {
            get
            {
                var fd = FormatedTextHelpers.FontData.FromResource("content");
                var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
                var fs = new FormattedString();
                fs.Spans.Add(new Span() { Text = "Dangerosité ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
                fs.Spans.Add(new Span() { Text = Item.Challenge, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
                return fs;
            }
        }



        //public FormattedString Description
        //{
        //    get
        //    {
        //        var fs = new FormattedString();
        //        if (Item?.DescriptionDiv != null)
        //        {
        //            FormatedTextHelpers.HtmlToFormatedString(Item?.DescriptionDiv, fs, FontAttributes.None);
        //        }
        //        return fs;
        //    }
        //}

        //public FormattedString TypeLevel
        //{
        //    get
        //    {
        //        var fd = FormatedTextHelpers.FontData.FromResource("contentital");
        //        var fs = new FormattedString();
        //        var capType = Item.Type.First().ToString().ToUpper() + Item.Type.Substring(1);
        //        fs.Spans.Add(new Span() { Text = string.Format("{0} de niveau {1}", capType, Item.Level), FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor});
        //        return fs;
        //    }
        //}

        //public FormattedString CastingTime
        //{
        //    get
        //    {
        //        var fd = FormatedTextHelpers.FontData.FromResource("content");
        //        var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
        //        var fs = new FormattedString();
        //        fs.Spans.Add(new Span() { Text = "Durée d'incantation : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
        //        fs.Spans.Add(new Span() { Text = Item.CastingTime, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
        //        return fs;
        //    }
        //}

        //public FormattedString Range
        //{
        //    get
        //    {
        //        var fd = FormatedTextHelpers.FontData.FromResource("content");
        //        var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
        //        var fs = new FormattedString();
        //        fs.Spans.Add(new Span() { Text = "Portée : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
        //        fs.Spans.Add(new Span() { Text = Item.Range, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
        //        return fs;
        //    }
        //}

        //public FormattedString Components
        //{
        //    get
        //    {
        //        var fd = FormatedTextHelpers.FontData.FromResource("content");
        //        var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
        //        var fs = new FormattedString();
        //        fs.Spans.Add(new Span() { Text = "Composantes : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
        //        fs.Spans.Add(new Span() { Text = Item.Components, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
        //        return fs;
        //    }
        //}

        //public FormattedString Duration
        //{
        //    get
        //    {
        //        var fd = FormatedTextHelpers.FontData.FromResource("content");
        //        var fdb = FormatedTextHelpers.FontData.FromResource("contentbold");
        //        var fs = new FormattedString();
        //        fs.Spans.Add(new Span() { Text = "Durée : ", FontFamily = fdb.FontFamily, FontAttributes = fdb.FontAttributes, FontSize = fdb.FontSize, ForegroundColor = fdb.TextColor });
        //        fs.Spans.Add(new Span() { Text = Item.Duration, FontFamily = fd.FontFamily, FontAttributes = fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor });
        //        return fs;
        //    }
        //}

        public Command LoadItemCommand { get; set; }

        public MonsterDetailViewModel(Monster item = null)
        {
            Title = item?.NamePHB;
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
                //Item.ParseHtml();
                //Item = _Item;
                //Items.Clear();
                //var item = await new MonstersScrappers().GetMonster(Item.Id);
                //Item = item;
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
