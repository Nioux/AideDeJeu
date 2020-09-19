using AideDeJeu.UWP;
using AideDeJeu.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: Xamarin.Forms.Platform.UWP.ExportRenderer(typeof(PdfView), typeof(PdfViewRenderer))]
namespace AideDeJeu.UWP
{
    public class PdfViewRenderer : WebViewRenderer
    {
        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Uri")
            {
                var pdfView = Element as PdfView;
                if (pdfView?.Uri != null)
                {
                    // TODO : copier le dossier assets/pdfjs dans localcache/pdf/pdfjs
                    await CopyPdfJSAsync();
                    Control.Source = new Uri(
                        //string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file={0}",
                        string.Format("ms-appdata:///localcache/pdf/pdfjs/web/viewer.html?file=../../{0}",
                        pdfView.Uri));
                        //WebUtility.UrlEncode(pdfView.Uri))));
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
                var pdfView = Element as PdfView;
                if (pdfView?.Uri != null)
                {
                    Control.Source = new Uri(string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file={0}", string.Format("ms-appx-web:///Assets/Content/{0}", WebUtility.UrlEncode(pdfView.Uri))));
                }
            }
        }

        private async Task CopyPdfJSAsync()
        {
            var temporaryFolder = ApplicationData.Current.LocalCacheFolder;
            if ((await temporaryFolder.TryGetItemAsync("pdf\\pdfjs")) == null)
            {
                var pdfjsDestinationFolder = await temporaryFolder.CreateFolderAsync("pdf\\pdfjs");
                var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var pdfjsSourceFolder = await installationFolder.GetFolderAsync("Assets\\pdfjs");
                await CopyFolderAsync(pdfjsSourceFolder, pdfjsDestinationFolder);
            }
        }

        private async Task CopyFolderAsync(StorageFolder sourceFolder, StorageFolder destinationFolder)
        {
            foreach(var folder in await sourceFolder.GetFoldersAsync())
            {
                await CopyFolderAsync(folder, await destinationFolder.CreateFolderAsync(folder.Name));
            }
            foreach(var file in await sourceFolder.GetFilesAsync())
            {
                await file.CopyAsync(destinationFolder);
            }
        }
    }
}
