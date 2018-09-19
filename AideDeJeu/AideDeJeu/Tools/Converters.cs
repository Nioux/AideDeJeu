using AideDeJeu.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using Xamarin.Forms;
using System.Linq;
using SkiaSharp;
using System.IO;
using System.Reflection;

namespace AideDeJeu.Tools
{
    public class NullToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class NullToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IntToValueConverter<T> : IValueConverter
    {
        public T NullOrZeroValue { get; set; }
        public T NonZeroValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                int? ivalue = value as int?;
                if(ivalue.HasValue && ivalue > 0)
                {
                    return NonZeroValue;
                }
            }
            return NullOrZeroValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IntToBooleanConverter : IntToValueConverter<bool> { }

    public class HeaderLevelToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var level = value as int?;
                if (level.HasValue)
                {
                    int finallevel = level.Value;
                    int baselevel = 1;
                    if (int.TryParse(parameter as string, out baselevel))
                    {
                        finallevel += baselevel;
                    }
                    finallevel = Math.Max(1, Math.Min(6, finallevel));
                    var heading = $"heading{finallevel}";
                    if (Application.Current.Resources.ContainsKey(heading))
                    {
                        return Application.Current.Resources[heading];
                    }
                }
            }
            catch(Exception)
            {

            }
            return Application.Current.Resources["paragraph"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SVGToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SkiaSharp.Extended.Svg.SKSvg svg = new SkiaSharp.Extended.Svg.SKSvg();
            var assembly = typeof(Helpers).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("AideDeJeu.test.svg"))
            {
                svg.Load(stream);
                //svg.Load(**Your SVG stream or file * *);
                using (SKBitmap bitmap = new SKBitmap((int)svg.CanvasSize.Width, (int)svg.CanvasSize.Height))
                using (SKCanvas canvas = new SKCanvas(bitmap))
                {
                    canvas.DrawPicture(svg.Picture);
                    canvas.Flush();
                    canvas.Save();

                    using (SKImage image = SKImage.FromBitmap(bitmap))
                    {
                        var encoded = image.Encode();
                        var sstream = encoded.AsStream();
                        var source = ImageSource.FromStream(() => sstream);

                        return source;
                    }
                    //using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                    //using (MemoryStream memStream = new MemoryStream())
                    //{
                    //    data.SaveTo(memStream);
                    //    memStream.Seek(0, SeekOrigin.Begin);
                    //    return memStream;
                    //    //using (SKManagedStream skStream = new SKManagedStream(memStream))
                    //    //{
                    //    //    return SKBitmap.Decode(skStream);
                    //    //}
                    //}
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
