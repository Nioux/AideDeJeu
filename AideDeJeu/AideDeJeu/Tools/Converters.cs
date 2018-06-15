using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
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
                var doc = new XmlDocument();
                doc.LoadXml("<div>" + str + "</div>");

                var fs = new FormattedString();
                FormatedTextHelpers.HtmlNodeToFormatedString(doc.DocumentElement, fs);
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
                    var doc = new XmlDocument();
                    doc.LoadXml("<div>" + str + "</div>");

                    FormatedTextHelpers.HtmlNodeToFormatedString(doc.DocumentElement, fs);
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


    public class ItemSourceTypeConverter<T> : IValueConverter
    {
        //public T SpellVF { get; set; }
        public T SpellVO { get; set; }
        public T SpellHD { get; set; }
        //public T MonsterVF { get; set; }
        public T MonsterVO { get; set; }
        public T MonsterHD { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var itemType = value as ItemSourceType?;
            //if (itemType == ItemSourceType.SpellVF)
            //{
            //    return SpellVF;
            //}
            if (itemType == ItemSourceType.SpellVO)
            {
                return SpellVO;
            }
            if (itemType == ItemSourceType.SpellHD)
            {
                return SpellHD;
            }
            //if (itemType == ItemSourceType.MonsterVF)
            //{
            //    return MonsterVF;
            //}
            if (itemType == ItemSourceType.MonsterVO)
            {
                return MonsterVO;
            }
            if (itemType == ItemSourceType.MonsterHD)
            {
                return MonsterHD;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ItemSourceTypeToStringConverter : ItemSourceTypeConverter<string> { }

    public class ItemSourceTypeToItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = DependencyService.Get<MainViewModel>();
            var itemSourceType = vm.ItemSourceType;
            return vm.GetItemsViewModel(itemSourceType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ItemSourceTypeToFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = DependencyService.Get<MainViewModel>();
            var itemSourceType = vm.ItemSourceType;
            return vm.GetFilterViewModel(itemSourceType).Filters;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
