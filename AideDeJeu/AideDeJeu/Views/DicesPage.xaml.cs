using AideDeJeu.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DicesPage : ContentPage
    {
        public DicesPage()
        {
            InitializeComponent();
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            var diceRoller = new DiceRollerViewModel();
            var diceRolls = diceRoller.DicesValues(6, 3);
            foreach (var diceRoll in diceRolls)
            {
                Debug.WriteLine($"{diceRoll.Key} => {diceRoll.Value / 3}");
            }


            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            SKPaint strokePaint = new SKPaint() { Color = new SKColor(0x9B, 0x1C, 0x47), Style = SKPaintStyle.Stroke, StrokeWidth = 2 };
            canvas.Clear();

            float minx = diceRolls.Min(kv => kv.Value);
            float miny = diceRolls.Min(kv => kv.Key);
            float maxx = diceRolls.Max(kv => kv.Value);
            float maxy = diceRolls.Max(kv => kv.Key);
            float sizey = info.Height / (maxy - miny + 1);
            foreach (var diceRoll in diceRolls)
            {
                float x = diceRoll.Value;
                float y = diceRoll.Key;
                canvas.DrawRect(new SKRect(0, ((y - miny) * sizey), x * info.Width / maxx, ((y - miny) * sizey) + sizey - 5), strokePaint);
            }
        }
    }
}