using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AideDeJeu.Views;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace AideDeJeu.iOS
{
    public class PdfViewRenderer : ViewRenderer<PdfView, UIWebView>
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName == "Uri")
            {
                var pdfView = Element as PdfView;

                if (pdfView?.Uri != null)
                {
                    LoadFile(pdfView.Uri);
                }
            }
        }
        protected override void OnElementChanged(ElementChangedEventArgs<PdfView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }
            if (e.OldElement != null)
            {
                // Cleanup
            }
            if (e.NewElement != null)
            {
                var pdfView = Element as PdfView;

                if (pdfView?.Uri != null)
                {
                    LoadFile(pdfView.Uri);
                }
            }
        }

        void LoadFile(string fileName)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Format("pdf/{0}", WebUtility.UrlEncode(fileName)));
            Control.LoadRequest(new NSUrlRequest(new NSUrl(filePath, false)));
            Control.ScalesPageToFit = true;
        }
    }
}