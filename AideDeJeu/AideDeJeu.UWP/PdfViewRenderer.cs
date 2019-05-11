using AideDeJeu.UWP;
using AideDeJeu.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(PdfView), typeof(PdfViewRenderer))]
namespace AideDeJeu.UWP
{
    public class PdfViewRenderer : WebViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Uri")
            {
                var pdfView = Element as PdfView;
                if (pdfView.Uri != null)
                {
                    Control.Source = new Uri(
                        string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file={0}", string.Format("file://{0}", WebUtility.UrlEncode(pdfView.Uri))));
                        //string.Format("ms-appx-web:///Assets/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
                    //Control.Settings.AllowFileAccess = true;
                    //Control.Settings.AllowFileAccessFromFileURLs = true;
                    //Control.Settings.AllowUniversalAccessFromFileURLs = true;
                    //Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
                    //Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file://{0}", WebUtility.UrlEncode(pdfView.Uri))));
                }
            }

        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as PdfView;
                Control.Source = new Uri(string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file={0}", string.Format("ms-appx-web:///Assets/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
            }
        }
    }
}
