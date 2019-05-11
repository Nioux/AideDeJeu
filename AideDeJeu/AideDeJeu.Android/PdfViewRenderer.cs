using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using AideDeJeu.Droid;
using AideDeJeu.Views;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PdfView), typeof(PdfViewRenderer))]
namespace AideDeJeu.Droid
{
    public class PdfViewRenderer : WebViewRenderer
    {
        public PdfViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Uri")
            {
                var pdfView = Element as PdfView;
                if (pdfView.Uri != null)
                {
                    Control.Settings.AllowFileAccess = true;
                    Control.Settings.AllowFileAccessFromFileURLs = true;
                    Control.Settings.AllowUniversalAccessFromFileURLs = true;
                    //Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
                    Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file://{0}", WebUtility.UrlEncode(pdfView.Uri))));
                }
            }

        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var pdfView = Element as PdfView;
                if (pdfView.Uri != null)
                {
                    Control.Settings.AllowFileAccess = true;
                    Control.Settings.AllowFileAccessFromFileURLs = true;
                    Control.Settings.AllowUniversalAccessFromFileURLs = true;
                    //Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
                    Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file://{0}", WebUtility.UrlEncode(pdfView.Uri))));
                }
            }
        }
    }
}