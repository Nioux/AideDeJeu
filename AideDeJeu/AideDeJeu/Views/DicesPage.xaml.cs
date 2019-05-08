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
            var vm = new DiceRollerViewModel();
            vm.PropertyChanged += Vm_PropertyChanged;
            BindingContext = vm;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanvasDices.InvalidateSurface();
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            var vm = BindingContext as DiceRollerViewModel;
            var diceRoller = new DiceRollerViewModel();
            var diceRolls = diceRoller.DicesValues(vm.Type, vm.Quantity);
            foreach (var diceRoll in diceRolls)
            {
                Debug.WriteLine($"{diceRoll.Key} => {diceRoll.Value / vm.Quantity}");
            }


            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            float minx = diceRolls.Min(kv => kv.Value);
            float miny = diceRolls.Min(kv => kv.Key);
            float maxx = diceRolls.Max(kv => kv.Value);
            float maxy = diceRolls.Max(kv => kv.Key);
            float sumx = diceRolls.Sum(kv => kv.Value);
            float sizey = info.Height / (maxy - miny + 1);

            SKTypeface typeface = null;
            var inFont = AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.LinLibertine_R.ttf");
            typeface = SKTypeface.FromStream(inFont);

            SKPaint strokePaint = new SKPaint()
            {
                Color = new SKColor(0x9B, 0x1C, 0x47),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
            };
            SKPaint strokeFont = new SKPaint()
            {
                Color = new SKColor(0x9B, 0x1C, 0x47),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                TextSize = sizey * 0.6f,
                Typeface = typeface
            };
            canvas.Clear();
            foreach (var diceRoll in diceRolls)
            {
                float x = diceRoll.Value;
                float y = diceRoll.Key;
                canvas.DrawRect(new SKRect(0, ((y - miny) * sizey), x * info.Width / maxx, ((y - miny) * sizey) + sizey - 5), strokePaint);

                canvas.DrawText($"{y} => {x / sumx * 100:0.00}%", 10, ((y - miny) * sizey) + sizey * 0.6f, strokeFont);
            }

            typeface.Dispose();
            strokeFont.Dispose();
            strokePaint.Dispose();
        }
    }
}