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

}
