using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AideDeJeu.Models;
using AideDeJeuLib;
using HtmlAgilityPack;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AideDeJeu.ViewModels
{
    public class SpellDetailViewModel : BaseViewModel
    {
        Spell _Item = null;
        public Spell Item
        {
            get { return _Item; }
            set { SetProperty(ref _Item, value); OnPropertyChanged(nameof(Description)); }
        }

        public FormattedString Description
        {
            get
            {
                var fs = new FormattedString();
                if (Item?.DescriptionDiv != null)
                {
                    HtmlToFormatedString(Item?.DescriptionDiv, fs, FontAttributes.None);
                }
                return fs;
            }
        }

        void HtmlToFormatedString(HtmlNode parentNode, FormattedString fs, FontAttributes attributes)
        {
            foreach (var node in parentNode.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Text)
                {
                    var resname = "content";
                    if (attributes.HasFlag(FontAttributes.Bold))
                    {
                        resname += "bold";
                    }
                    if (attributes.HasFlag(FontAttributes.Italic))
                    {
                        resname += "ital";
                    }
                    var fd = FontData.FromResource(resname);
                    fs.Spans.Add(new Span() { FontFamily = fd.FontFamily, FontAttributes = attributes | fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor, Text = node.InnerText });
                }
                else if (node.NodeType == HtmlNodeType.Element && node.Name == "br")
                {
                    fs.Spans.Add(new Span() { Text = "\r\n" });
                }
                else if (node.NodeType == HtmlNodeType.Element && node.Name == "strong")
                {
                    HtmlToFormatedString(node, fs, attributes | FontAttributes.Bold);
                }
                else if (node.NodeType == HtmlNodeType.Element && node.Name == "em")
                {
                    HtmlToFormatedString(node, fs, attributes | FontAttributes.Italic);
                }
                else if (node.NodeType == HtmlNodeType.Element)
                {
                    HtmlToFormatedString(node, fs, attributes);
                }
            }
        }
        public Command LoadItemCommand { get; set; }

        public SpellDetailViewModel(Spell item = null)
        {
            Title = item?.Title;
            Item = item;
            LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
        }
        async Task ExecuteLoadItemCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //Items.Clear();
                var item = await new Scrappers().GetSpell(Item.Id);
                Item = item;
                //foreach (var item in items)
                //{
                //    Items.Add(item);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }




    public class FontData
    {
        public double FontSize { get; set; }
        public FontAttributes FontAttributes { get; set; }
        public Color TextColor { get; set; }
        public string FontFamily { get; set; }

        public static FontData DefaultValues()
        {
            return new FontData
            {
                FontSize = (double)Label.FontSizeProperty.DefaultValue,
                FontAttributes = (FontAttributes)Label.FontAttributesProperty.DefaultValue,
                TextColor = (Color)Label.TextColorProperty.DefaultValue,
                FontFamily = Label.FontFamilyProperty.DefaultValue.ToString()
            };
        }

        public static FontData FromResource(string resourceName)
        {
            var resource = Application.Current.Resources[resourceName];
            if (resource == null)
            {
                return DefaultValues();
            }
            var style = (Style)resource;

            var data = new FontData();
            var colorSetter = style.Setters.FirstOrDefault(x => x.Property == Label.TextColorProperty);
            var attrSetter = style.Setters.FirstOrDefault(x => x.Property == Label.FontAttributesProperty);
            var fontSizeSetter = style.Setters.FirstOrDefault(x => x.Property == Label.FontSizeProperty);
            var fontFamilySetter = style.Setters.FirstOrDefault(x => x.Property == Label.FontFamilyProperty);
            var platformFontFamilySetter = ResolveSetterToClass<string>(fontFamilySetter);
            var platformColorSetter = ResolveSetterToStruct<Color>(colorSetter);

            if (platformColorSetter.HasValue)
            {
                data.TextColor = platformColorSetter.Value;
            }
            else
            { 
                data.TextColor = colorSetter?.Value as Color? ?? (Color)Label.TextColorProperty.DefaultValue;
            }
            data.FontSize = fontSizeSetter?.Value as double? ?? (double)Label.FontSizeProperty.DefaultValue;

            
            if (platformFontFamilySetter != null)
            {
                data.FontFamily = platformFontFamilySetter;
            }
            else
            { 
                data.FontFamily = fontFamilySetter != null && fontFamilySetter.Value != null
                    ? fontFamilySetter.Value.ToString()
                    : Label.FontFamilyProperty.DefaultValue?.ToString();
            }

            data.FontAttributes = attrSetter?.Value != null
                ? (FontAttributes)Enum.Parse(typeof(FontAttributes), attrSetter.Value.ToString())
                : (FontAttributes)Label.FontAttributesProperty.DefaultValue;

            return data;
        }

        static T ResolveOnPlatformToClass<T>(string key) where T : class
        {
            var resource = Application.Current.Resources[key];
            return resource as OnPlatform<T>;
        }
        static T ResolveSetterToClass<T>(Setter setter) where T : class
        {
            if (setter?.Value is DynamicResource)
            {
                var res = setter.Value as DynamicResource;
                return ResolveOnPlatformToClass<T>(res.Key);
            }
            else
            {
                return setter?.Value as T;
            }
        }

        static Nullable<T> ResolveOnPlatformToStruct<T>(string key) where T : struct
        {
            var resource = Application.Current.Resources[key];
            return resource as OnPlatform<Nullable<T>>;
        }
        static Nullable<T> ResolveSetterToStruct<T>(Setter setter) where T : struct
        {
            if(setter?.Value is DynamicResource)
            {
                var res = setter.Value as DynamicResource;
                return ResolveOnPlatformToStruct<T>(res.Key);
            }
            else
            {
                return setter?.Value as T?;
            }
        }
    }
}
