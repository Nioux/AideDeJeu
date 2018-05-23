using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

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

    public class HtmlNodeToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str != null)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(str);

                var fs = new FormattedString();
                FormatedTextHelpers.HtmlNodeToFormatedString(doc.DocumentNode, fs);
                return fs;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class HtmlNodesToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strings = value as IEnumerable<string>;
            if (strings != null)
            {
                var fs = new FormattedString();
                foreach (var str in strings)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(str);

                    FormatedTextHelpers.HtmlNodeToFormatedString(doc.DocumentNode, fs);
                    fs.Spans.Add(new Span() { Text = "\r\n" });
                }
                return fs;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ItemsTypeTemplateConverter : IValueConverter
    {
        public ControlTemplate SpellsTemplate { get; set; }
        public ControlTemplate MonstersTemplate { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var itemType = value as ViewModels.MainViewModel.ItemType?;
            if (itemType == ViewModels.MainViewModel.ItemType.Spell)
            {
                return SpellsTemplate;
            }
            if (itemType == ViewModels.MainViewModel.ItemType.Monster)
            {
                return MonstersTemplate;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ItemTypeConverter<T> : IValueConverter
    {
        public T Spells { get; set; }
        public T Monsters { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var itemType = value as ViewModels.MainViewModel.ItemType?;
            if (itemType == ViewModels.MainViewModel.ItemType.Spell)
            {
                return Spells;
            }
            if (itemType == ViewModels.MainViewModel.ItemType.Monster)
            {
                return Monsters;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ItemTypeToStringConverter : ItemTypeConverter<string> { }
}
