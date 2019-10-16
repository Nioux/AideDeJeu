using AideDeJeu.ViewModels;
using OnePlat.DiceNotation;
using OnePlat.DiceNotation.DieRoller;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
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
            //CanvasDices.InvalidateSurface();
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
        Charts charts = null;
        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            try
            {
                var ao = new Urho.ApplicationOptions(assetsFolder: null);
                HelloWorldUrhoSurface = new Urho.Forms.UrhoSurface();
                charts = await HelloWorldUrhoSurface.Show<Charts>(ao);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            try
            {
                if (!Accelerometer.IsMonitoring)
                {
                    Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
                    Accelerometer.Start(SensorSpeed.Game);
                }
            }
            catch
            { }

        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            charts?.Bars.ForEach(b => b.SetValueWithAnimation((new Dice()).Roll("3d6", new RandomDieRoller()).Value));
        }

        private void Refresh_Clicked(object sender, EventArgs e)
        {
            charts?.Bars.ForEach(b => b.SetValueWithAnimation((new Dice()).Roll("3d6", new RandomDieRoller()).Value));
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
                }
            }
            catch { }
        }

        private void Menu_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//home", true);
        }
    }
    public class HelloWorld : Urho.Application
    {
        public HelloWorld(Urho.ApplicationOptions options) : base(options)
        {

        }
        protected override void Start()
        {
            base.Start();
            CreateText();
        }

        private void CreateText()
        {
            // Create Text Element
            var text = new Urho.Gui.Text()
            {
                Value = "Hello World!",
                HorizontalAlignment = Urho.Gui.HorizontalAlignment.Center,
                VerticalAlignment = Urho.Gui.VerticalAlignment.Center
            };

            text.SetColor(Urho.Color.Cyan);
            text.SetFont(font: ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);
            // Add to UI Root
            UI.Root.AddChild(text);
        }
    }


}