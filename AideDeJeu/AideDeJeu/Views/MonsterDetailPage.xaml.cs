
using AideDeJeu.ViewModels;
using AideDeJeuLib.Monsters;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using AideDeJeuLib.Spells;
using System.Text;
using System.Globalization;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonsterDetailPage : ContentPage
	{
        MonsterDetailViewModel viewModel;

        public MonsterDetailPage(MonsterDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            mdSpecialFeatures.NavigateToLink = async(s) => await NavigateToLink(s);
            mdActions.NavigateToLink = async (s) => await NavigateToLink(s);
        }

        public async Task NavigateToLink(string s)
        {
            var regex = new Regex("/(?<file>.*)\\.md#(?<anchor>.*)");
            var match = regex.Match(s);
            var file = match.Groups["file"].Value;
            var anchor = match.Groups["anchor"].Value;
            if (file == "spells_hd")
            {
                var spells = await viewModel.Main.GetItemsViewModel(ItemSourceType.SpellHD).GetAllItemsAsync();
                var spell = spells.Where(i => IdFromName(i.Id) == anchor).FirstOrDefault();
                var page = new SpellDetailPage(new SpellDetailViewModel(spell as Spell));
                await Navigation.PushAsync(page);
            }
            else if (file == "monsters_hd")
            {
                var monsters = await viewModel.Main.GetItemsViewModel(ItemSourceType.MonsterHD).GetAllItemsAsync();
                var monster = monsters.Where(i => IdFromName(i.Id) == anchor).FirstOrDefault();
                var page = new MonsterDetailPage(new MonsterDetailViewModel(monster as Monster));
                await Navigation.PushAsync(page);
            }
            //Device.OpenUri(new Uri(s));
        }

        public static string Capitalize(string text)
        {
            return string.Concat(text.Take(1)).ToUpper() + string.Concat(text.Skip(1)).ToString().ToLower();
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        static string IdFromName(string name)
        {
            return RemoveDiacritics(name.ToLower().Replace(" ", "-"));
        }


        public MonsterDetailPage()
        {
            InitializeComponent();

            var item = new Monster
            {
                Name = "",
                NameVO = "",
                NamePHB = "",
                //Description = "This is an item description."
            };

            viewModel = new MonsterDetailViewModel(item);
            BindingContext = viewModel;
        }

        void PaintHeaderBar(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };
            path.AddRect(new SKRect(0, 0, info.Width, 8));

            SKPaint paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(0xFFE69A28)
            };

            canvas.DrawPath(path, paint);

            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 2;
            paint.Color = SKColors.Black;

            canvas.DrawPath(path, paint);
            paint.Dispose();
            path.Dispose();
        }

        void PaintRedBar(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };
            path.AddPoly(new SKPoint[] { new SKPoint(0,0), new SKPoint(info.Width, 8), new SKPoint(0, 8) });

            SKPaint paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(0xFF922610)
            };

            canvas.DrawPath(path, paint);
            paint.Dispose();
            path.Dispose();

            //paint.Style = SKPaintStyle.Stroke;
            //paint.StrokeWidth = 2;
            //paint.Color = SKColors.Black;

            //canvas.DrawPath(path, paint);
        }
    }
}