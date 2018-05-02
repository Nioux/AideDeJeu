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
            var node = value as HtmlNode;
            if (node != null)
            {
                var fs = new FormattedString();
                FormatedTextHelpers.HtmlNodeToFormatedString(node, fs);
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
            var nodes = value as IEnumerable<HtmlNode>;
            if (nodes != null)
            {
                var fs = new FormattedString();
                foreach (var node in nodes)
                {
                    FormatedTextHelpers.HtmlNodeToFormatedString(node, fs);
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
            var itemType = value as ViewModels.ItemsViewModel.ItemType?;
            if (itemType == ViewModels.ItemsViewModel.ItemType.Spell)
            {
                return SpellsTemplate;
            }
            if (itemType == ViewModels.ItemsViewModel.ItemType.Monster)
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



}
