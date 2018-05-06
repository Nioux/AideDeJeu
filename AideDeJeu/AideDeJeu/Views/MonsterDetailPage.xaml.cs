
using AideDeJeu.ViewModels;
using AideDeJeuLib.Monsters;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        }

        public MonsterDetailPage()
        {
            InitializeComponent();

            var item = new Monster
            {
                Name = "",
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