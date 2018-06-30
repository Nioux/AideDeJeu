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

    public class StringListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strings = value as IEnumerable<string>;
            if (strings != null)
            {
                var cstring = string.Empty;
                foreach (var str in strings)
                {
                    cstring += str + "\n\n";
                }
                return cstring;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class ItemSourceTypeConverter<T> : IValueConverter
    {
        public T SpellVO { get; set; }
        public T SpellHD { get; set; }
        public T MonsterVO { get; set; }
        public T MonsterHD { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var itemType = value as ItemSourceType?;
            if (itemType == ItemSourceType.SpellVO)
            {
                return SpellVO;
            }
            if (itemType == ItemSourceType.SpellHD)
            {
                return SpellHD;
            }
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
