using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AideDeJeu.Tools
{
    public static class FormatedTextHelpers
    {
        public static void HtmlNodesToFormatedString(HtmlNodeCollection nodes, FormattedString fs, FontAttributes attributes = FontAttributes.None)
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    HtmlNodeToFormatedString(node, fs, attributes);
                }
            }
        }

        public static void HtmlNodeToFormatedString(HtmlNode node, FormattedString fs, FontAttributes attributes = FontAttributes.None)
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
                HtmlNodesToFormatedString(node.ChildNodes, fs, attributes | FontAttributes.Bold);
            }
            else if (node.NodeType == HtmlNodeType.Element && node.Name == "em")
            {
                HtmlNodesToFormatedString(node.ChildNodes, fs, attributes | FontAttributes.Italic);
            }
            else if (node.NodeType == HtmlNodeType.Element)
            {
                HtmlNodesToFormatedString(node.ChildNodes, fs, attributes);
            }
        }

        //public static void HtmlToFormatedString(HtmlNode parentNode, FormattedString fs, FontAttributes attributes = FontAttributes.None)
        //{
        //    if (parentNode.NodeType == HtmlNodeType.Element)
        //    {
        //        foreach (var node in parentNode.ChildNodes)
        //        {
        //            if (node.NodeType == HtmlNodeType.Text)
        //            {
        //                var resname = "content";
        //                if (attributes.HasFlag(FontAttributes.Bold))
        //                {
        //                    resname += "bold";
        //                }
        //                if (attributes.HasFlag(FontAttributes.Italic))
        //                {
        //                    resname += "ital";
        //                }
        //                var fd = FontData.FromResource(resname);
        //                fs.Spans.Add(new Span() { FontFamily = fd.FontFamily, FontAttributes = attributes | fd.FontAttributes, FontSize = fd.FontSize, ForegroundColor = fd.TextColor, Text = node.InnerText });
        //            }
        //            else if (node.NodeType == HtmlNodeType.Element && node.Name == "br")
        //            {
        //                fs.Spans.Add(new Span() { Text = "\r\n" });
        //            }
        //            else if (node.NodeType == HtmlNodeType.Element && node.Name == "strong")
        //            {
        //                HtmlToFormatedString(node, fs, attributes | FontAttributes.Bold);
        //            }
        //            else if (node.NodeType == HtmlNodeType.Element && node.Name == "em")
        //            {
        //                HtmlToFormatedString(node, fs, attributes | FontAttributes.Italic);
        //            }
        //            else if (node.NodeType == HtmlNodeType.Element)
        //            {
        //                HtmlToFormatedString(node, fs, attributes);
        //            }
        //        }
        //    }
        //    else if (parentNode.NodeType == HtmlNodeType.Text)
        //    {

        //    }
        //}

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
                if (setter?.Value is DynamicResource)
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
}
