using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AideDeJeu.Views
{
    public class PdfView : WebView
    {
        //public static readonly BindableProperty UriProperty = 
        //    BindableProperty.Create<PdfView, string>(p => p.Uri, default(string));
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            nameof(Uri),
            typeof(string),
            typeof(PdfView),
            defaultValue: default(string),
            propertyChanged: OnUriChanged);

        static void OnUriChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as PdfView;
        }
        //public string Uri
        //{
        //    get { return (string)GetValue(UriProperty); }
        //    set { SetValue(UriProperty, value); }
        //}
    }
}
