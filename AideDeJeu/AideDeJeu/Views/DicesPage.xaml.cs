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
            SKPaint strokePaint = new SKPaint() { Color = new SKColor(255,0,0,128) };
            canvas.Clear();

            // Draw path with cubic Bezier curve
            using (SKPath path = new SKPath())
            {
                path.MoveTo(0, 0);
                foreach (var diceRoll in diceRolls)
                {

                    path.LineTo(diceRoll.Key * 20, diceRoll.Value * 20);
                }
                canvas.DrawPath(path, strokePaint);
            }
        }
    }
}