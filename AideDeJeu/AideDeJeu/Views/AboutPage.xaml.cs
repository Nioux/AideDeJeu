using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Markup;

[assembly: ExportFont("AlverataIrregularPEMedium.ttf")]
[assembly: ExportFont("Cinzel-Bold.otf")]
[assembly: ExportFont("Cinzel-Regular.otf")]
[assembly: ExportFont("LinLibertine_R.ttf")]
[assembly: ExportFont("LinLibertine_RB.ttf")]
[assembly: ExportFont("LinLibertine_RBI.ttf")]
[assembly: ExportFont("LinLibertine_RI.ttf")]
[assembly: ExportFont("MarcellusSC-Regular.ttf")]
[assembly: ExportFont("MinionPro_It.ttf")]


namespace AideDeJeu.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			InitializeComponent ();

			//BindingContext = new ViewModels.AboutViewModel();

			//string LinuxLibertineCapitalsBold = "Cinzel-Bold";
			///*			switch (Device.RuntimePlatform)
			//			{
			//				case Device.iOS:
			//					LinuxLibertineCapitalsBold = "Linux Libertine Capitals";
			//					break;
			//				case Device.Android:
			//					LinuxLibertineCapitalsBold = "Cinzel-Bold.otf#Cinzel Bold";
			//					break;
			//				default:
			//					LinuxLibertineCapitalsBold = "Assets/Fonts/Cinzel-Bold.otf#Cinzel";
			//					break;
			//			}*/

			//var DarkHDRed = Color.FromHex("#9B1C47");
			//var LightHDRed = Color.FromHex("#9B1C47");

			//var heading1Style = new Style<Label>(
			//(Label.FontSizeProperty, 30),
			//(Label.TextColorProperty, App.Current.UserAppTheme == OSAppTheme.Dark ? DarkHDRed : LightHDRed),
			////(Label.TextColorProperty, "{AppThemeBinding Dark={StaticResource DarkHDRed}, Light={StaticResource LightHDRed}}"),
			//(Label.FontFamilyProperty, LinuxLibertineCapitalsBold),
			//(Label.FontAttributesProperty, FontAttributes.Bold)
			//);

			//Content = new Grid 
			//{ 
			//	ColumnSpacing = 0,
			//	RowDefinitions = 
			//	{ 
			//		new RowDefinition { Height = GridLength.Auto },
			//		new RowDefinition { Height = GridLength.Star }
			//	},
			//	ColumnDefinitions = 
			//	{
			//		new ColumnDefinition { Width = GridLength.Auto },
			//		new ColumnDefinition { Width = GridLength.Star }
			//	},
			//	Children =
			//	{
			//		new StackLayout
			//		{
			//			VerticalOptions = LayoutOptions.FillAndExpand,
			//			HorizontalOptions = LayoutOptions.Fill,
			//			Children =
			//			{
			//				new StackLayout
			//				{
			//					Orientation = StackOrientation.Horizontal,
			//					HorizontalOptions = LayoutOptions.Center,
			//					VerticalOptions = LayoutOptions.Center,
			//					Children =
			//					{
			//						new ContentView
			//						{
			//							Padding = new Thickness(20, 20, 20, 20),
			//							VerticalOptions = LayoutOptions.FillAndExpand,
			//							Content = new Image { VerticalOptions = LayoutOptions.Center, HeightRequest = 64, Source = "main.png" }
			//						}
			//					}
			//				}
			//			}
			//		},
			//		new StackLayout 
			//		{ 
			//			VerticalOptions = LayoutOptions.FillAndExpand, 
			//			HorizontalOptions = LayoutOptions.Fill,
			//			Children = 
			//			{
			//				new StackLayout 
			//				{ 
			//					Padding= new Thickness(20, 20, 20, 20),
			//					Orientation = StackOrientation.Vertical,
			//					HorizontalOptions = LayoutOptions.Center,
			//					VerticalOptions = LayoutOptions.Center,
			//					Children = 
			//					{
			//						new Label { Text="{Binding AppName}", Style = heading1Style, FontSize =  32, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center }
			//						.Bind(Label.TextProperty, nameof(ViewModels.AboutViewModel.AppName)),
			//						new Label { Text="{Binding Version, StringFormat=' Version {0}', Mode=OneTime}", HorizontalOptions = LayoutOptions.Center }
			//						.Bind(Label.TextProperty, nameof(ViewModels.AboutViewModel.Version), convert: (string version) => string.Format(" Version {0}", version))
			//					}
			//				}
			//			}
			//		}.Column(1).Row(0)
			//	}
			//};
		}
	}
}