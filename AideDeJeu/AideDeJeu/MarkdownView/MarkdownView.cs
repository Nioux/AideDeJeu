namespace Xam.Forms.Markdown
{
    using System.Linq;
    using Markdig.Syntax;
    using Markdig.Syntax.Inlines;
    using Xamarin.Forms;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Extensions;
    using Markdig;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class MarkdownView : ContentView
    {
        //public Func<string, Task> NavigateToLink { get; set; } = async(s) => Device.OpenUri(new Uri(s));

        public static MarkdownTheme Global = new LightMarkdownTheme();

        public string Markdown
        {
            get { return (string)GetValue(MarkdownProperty); }
            set { SetValue(MarkdownProperty, value); }
        }

        public static readonly BindableProperty MarkdownProperty = BindableProperty.Create(nameof(Markdown), typeof(string), typeof(MarkdownView), null, propertyChanged: OnMarkdownChanged);

        public string RelativeUrlHost
        {
            get { return (string)GetValue(RelativeUrlHostProperty); }
            set { SetValue(RelativeUrlHostProperty, value); }
        }

        public static readonly BindableProperty RelativeUrlHostProperty = BindableProperty.Create(nameof(RelativeUrlHost), typeof(string), typeof(MarkdownView), null, propertyChanged: OnMarkdownChanged);

        public MarkdownTheme Theme
        {
            get { return (MarkdownTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        public static readonly BindableProperty ThemeProperty = BindableProperty.Create(nameof(Theme), typeof(MarkdownTheme), typeof(MarkdownView), Global, propertyChanged: OnMarkdownChanged);


        public ICommand NavigateToLinkCommand
        {
            get { return (ICommand)GetValue(NavigateToLinkCommandProperty); }
            set { SetValue(NavigateToLinkCommandProperty, value); }
        }

        public static readonly BindableProperty NavigateToLinkCommandProperty = BindableProperty.Create(nameof(NavigateToLinkCommand), typeof(ICommand), typeof(MarkdownView)); //, Global, propertyChanged: OnMarkdownChanged);



        private bool isQuoted;

        private List<View> queuedViews = new List<View>();

        static void OnMarkdownChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as MarkdownView;
            view.RenderMarkdown();
        }

        private StackLayout _Stack;

        private List<KeyValuePair<string, string>> links = new List<KeyValuePair<string, string>>();

        private void RenderMarkdown()
        {
            _Stack = new StackLayout()
            {
                Spacing = this.Theme.Margin,
            };

            this.Padding = this.Theme.Margin;

            this.BackgroundColor = this.Theme.BackgroundColor;

            if(!string.IsNullOrEmpty(this.Markdown))
            {
                var pipeline = new Markdig.MarkdownPipelineBuilder().UseYamlFrontMatter().UsePipeTables().Build();
                var parsed = Markdig.Markdown.Parse(this.Markdown, pipeline);
                var views = this.Render(parsed.AsEnumerable());
                views.ToList().ForEach(view => _Stack.Children.Add(view));
            }

            this.Content = _Stack;

        }

        private IEnumerable<View> Render(IEnumerable<Block> blocks)
        {
            var views = new List<View>();
            foreach (var block in blocks)
            {
                var subviews = this.Render(block);
                if(subviews != null)
                {
                    views.AddRange(subviews);
                    //views.Add(view);
                }
            }
            return views;
        }

        private void AttachLinks(View view)
        {
            if (links.Any())
            {
                var blockLinks = links.Distinct().OrderBy(l => l.Key).ToList();
                if (blockLinks.Count > 1 && Device.RuntimePlatform == Device.GTK)
                { 
                    view.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(async () =>
                        {
                            try
                            {
                                var result = await Application.Current.MainPage.DisplayActionSheet("Ouvrir le lien", "Annuler", null, blockLinks.Select(x => x.Key).ToArray());
                                var link = blockLinks.FirstOrDefault(x => x.Key == result);
                                NavigateToLinkCommand?.Execute(link.Value);
                            }
                            catch (Exception) { }
                        }),
                    });
                }
                else
                {
                    view.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            try
                            {
                                NavigateToLinkCommand?.Execute(blockLinks.First().Value);
                            }
                            catch (Exception) { }
                        }),
                    });
                }
                links = new List<KeyValuePair<string, string>>();
            } 
        }

        private void AttachLink(Span span, KeyValuePair<string, string> link)
        {
            span.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    try
                    {
                        NavigateToLinkCommand?.Execute(link.Value);
                    }
                    catch (Exception)
                    {
                    }
                }),
            });
        }

        #region Rendering blocks

        private IEnumerable<View> Render(Block block)
        {
            var views = new List<View>();
            IEnumerable<View> subviews = null;
            switch (block)
            {
                case HeadingBlock heading:
                    subviews = Render(heading);
                    break;

                case ParagraphBlock paragraph:
                    subviews = Render(paragraph);
                    break;

                case QuoteBlock quote:
                    subviews = Render(quote);
                    break;

                case CodeBlock code:
                    subviews = Render(code);
                    break;

                case ListBlock list:
                    subviews = Render(list);
                    break;

                case ThematicBreakBlock thematicBreak:
                    subviews = Render(thematicBreak);
                    break;

                case HtmlBlock html:
                    subviews = Render(html);
                    break;

                case Markdig.Extensions.Tables.Table table:
                    subviews = Render(table);
                    break;

                default:
                    Debug.WriteLine($"Can't render {block.GetType()} blocks.");
                    break;
            }
            if(subviews != null)
            {
                views.AddRange(subviews);
            }

            if (views != null)
            {
                if (queuedViews.Any())
                {
                    foreach (var view in queuedViews)
                    {
                        views.Add(view);
                    }
                    queuedViews.Clear();
                }
            }
            return views;
        }

        private int listScope;

        private IEnumerable<View> Render(ThematicBreakBlock block)
        {
            var style = this.Theme.Separator;

            if (style.BorderSize > 0)
            {
                return new View[] { new BoxView
                {
                    HeightRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                } };
            }
            return null;
        }

        private IEnumerable<View> Render(ListBlock block)
        {
            listScope++;

            var views = new List<View>();
            for (int i = 0; i < block.Count(); i++)
            {
                var item = block.ElementAt(i);

                if (item is ListItemBlock itemBlock)
                {
                    var subviews = this.Render(block, i + 1, itemBlock);
                    if(subviews != null)
                    {
                        views.AddRange(subviews);
                    }
                }
            }

            listScope--;
            return views;
        }

        private IEnumerable<View> Render(ListBlock parent, int index, ListItemBlock block)
        {
            //var initialStack = this.stack;

            var stack = new StackLayout()
            {
                Spacing = this.Theme.Margin,
            };

            var subv = this.Render(block.AsEnumerable());
            subv.ToList().ForEach(v => stack.Children.Add(v));

            var horizontalStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(listScope * this.Theme.Margin, 0, 0, 0),
            };

            if (parent.BulletType != '-')
            {
                View bullet;

                if (parent.IsOrdered)
                {
                    bullet = new Label
                    {
                        Text = $"{index}.",
                        FontSize = this.Theme.Paragraph.FontSize,
                        TextColor = this.Theme.Paragraph.ForegroundColor,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.End,
                    };
                }
                else
                {
                    bullet = new Label
                    {
                        Text = "●",
                        FontSize = this.Theme.Paragraph.FontSize,
                        TextColor = this.Theme.Paragraph.ForegroundColor,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.End,
                    };
                    //bullet = new BoxView
                    //{
                    //    WidthRequest = 4,
                    //    HeightRequest = 4,
                    //    Margin = new Thickness(0, 6, 0, 0),
                    //    BackgroundColor = this.Theme.Paragraph.ForegroundColor,
                    //    VerticalOptions = LayoutOptions.Start,
                    //    HorizontalOptions = LayoutOptions.Center,
                    //};
                }

                horizontalStack.Children.Add(bullet);
            }

            horizontalStack.Children.Add(stack);
            //initialStack.Children.Add(horizontalStack);

            //this.stack = initialStack;
            return new View[] { horizontalStack };
        }

        private IEnumerable<View> Render(HeadingBlock block)
        {
            MarkdownStyle style;

            switch (block.Level)
            {
                case 1:
                    style = this.Theme.Heading1;
                    break;
                case 2:
                    style = this.Theme.Heading2;
                    break;
                case 3:
                    style = this.Theme.Heading3;
                    break;
                case 4:
                    style = this.Theme.Heading4;
                    break;
                case 5:
                    style = this.Theme.Heading5;
                    break;
                default:
                    style = this.Theme.Heading6;
                    break;
            }

            var foregroundColor = isQuoted ? this.Theme.Quote.ForegroundColor : style.ForegroundColor;

            var label = new Label
            {
                FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, foregroundColor, style.BackgroundColor, style.FontSize),
            };

            AttachLinks(label);

            if (style.BorderSize > 0)
            {
                var headingStack = new StackLayout();
                headingStack.Children.Add(label);
                headingStack.Children.Add(new BoxView
                {
                    HeightRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });
                return new View[] { headingStack };
            }
            else
            {
                return new View[] { label };
            }
        }

        private IEnumerable<View> Render(ParagraphBlock block)
        { 
            var style = this.Theme.Paragraph;
            var foregroundColor = isQuoted ? this.Theme.Quote.ForegroundColor : style.ForegroundColor;
            var label = new Label
            {
                FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, foregroundColor, style.BackgroundColor, style.FontSize),
            };
            AttachLinks(label);
            return new View[] { label };
        }

        private IEnumerable<View> Render(HtmlBlock block)
        {
            if(block.Type == HtmlBlockType.NonInterruptingBlock || block.Type == HtmlBlockType.Comment)
            {
                var tag = block.Lines.Lines.FirstOrDefault().Slice.ToString();
                if (tag == "<!--br-->" || tag == "<br>")
                {
                    var label = new Label
                    {
                        Text = "\n",
                    };
                    return new View[] { label };

                }
                else
                {
                    // ?
                }
            }
            else
            {
                // ?
            }
            // ?
            return null;
        }

        private IEnumerable<View> Render(QuoteBlock block)
        {
            var initialIsQuoted = this.isQuoted;
            //var initialStack = this.stack;
            var views = new List<View>();

            this.isQuoted = true;
            var stack = new StackLayout()
            {
                Spacing = this.Theme.Margin,
            };

            var style = this.Theme.Quote;

            if (style.BorderSize > 0)
            {
                var horizontalStack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    BackgroundColor = this.Theme.Quote.BackgroundColor,
                };

                horizontalStack.Children.Add(new BoxView()
                {
                    WidthRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });

                horizontalStack.Children.Add(stack);
                views.Add(horizontalStack);
            }
            else
            {
                stack.BackgroundColor = this.Theme.Quote.BackgroundColor;
                views.Add(stack);
            }

            var subviews = this.Render(block.AsEnumerable());

            this.isQuoted = initialIsQuoted;
            //this.stack = initialStack;
            return subviews;
        }

        private IEnumerable<View> Render(CodeBlock block)
        {
            var views = new List<View>();
            var style = this.Theme.Code;
            var label = new Label
            {
                TextColor = style.ForegroundColor,
                FontAttributes = style.Attributes,
                FontFamily = style.FontFamily,
                FontSize = style.FontSize,
                Text = string.Join(Environment.NewLine, block.Lines),
            };
            views.Add(new Frame()
            {
                CornerRadius = 3,
                HasShadow = false,
                Padding = this.Theme.Margin,
                BackgroundColor = style.BackgroundColor,
                Content = label
            });
            return views;
        }

        private IEnumerable<View> Render(Markdig.Extensions.Tables.Table tableBlock)
        {
            var scroll = new ScrollView() { HorizontalScrollBarVisibility = ScrollBarVisibility.Default, Orientation = ScrollOrientation.Horizontal };
            var grid = new Grid() { HorizontalOptions = LayoutOptions.Start, Margin = 0, Padding = 1, BackgroundColor = Theme.TableHeader.BackgroundColor, RowSpacing = 1, ColumnSpacing = 1 };

            int maxColumns = 0;
            foreach (Markdig.Extensions.Tables.TableRow row in tableBlock)
            {
                maxColumns = Math.Max(maxColumns, row.Count);
                grid.RowDefinitions.Add(new RowDefinition { Height= new GridLength(1, GridUnitType.Auto) });
            }
            for (int i = 0; i < maxColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            }




            int top = 0;
            foreach (Markdig.Extensions.Tables.TableRow row in tableBlock)
            {
                int left = 0;
                foreach (Markdig.Extensions.Tables.TableCell cell in row)
                {
                    foreach (var blockpar in cell)
                    {
                        var par = blockpar as Markdig.Syntax.ParagraphBlock;
                        var style = row.IsHeader ? Theme.TableHeader : (top%2==1) ? Theme.TableCell : Theme.TableCellAlt;
                        var frame = new Frame
                        {
                            BackgroundColor = style.BackgroundColor,
                            Margin = 1,
                            Padding = 5,
                        };
                        //var align = tableBlock.ColumnDefinitions[left].Alignment;
                        var layoutOptions = row.IsHeader ? LayoutOptions.CenterAndExpand : LayoutOptions.StartAndExpand;
                        var textAlignment = row.IsHeader ? TextAlignment.Center : TextAlignment.Start;
                        var label = new Label
                        {
                            FormattedText = CreateFormatted(par.Inline, style.FontFamily, style.Attributes, style.ForegroundColor, style.BackgroundColor, style.FontSize),
                            HorizontalOptions = layoutOptions,
                            BackgroundColor = style.BackgroundColor,
                            LineBreakMode = LineBreakMode.WordWrap,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = textAlignment,
                        };
                        frame.Content = label;
                        AttachLinks(label);
                        grid.Children.Add(frame, left, top);
                    }

                    left++;
                }
                top++;
            }
            //stack.Children.Add(scroll);
            scroll.Content = grid;
            return new View[] { scroll };
        }


        private FormattedString CreateFormatted(ContainerInline inlines, string family, FontAttributes attributes, Color foregroundColor, Color backgroundColor, float size)
        {
            var fs = new FormattedString();

            foreach (var inline in inlines)
            {
                var spans = CreateSpans(inline, family, attributes, foregroundColor, backgroundColor, size);
                if (spans != null)
                {
                    foreach (var span in spans)
                    {
                        fs.Spans.Add(span);
                    }
                }
            }

            return fs;
        }

        private Span[] CreateSpans(Inline inline, string family, FontAttributes attributes, Color foregroundColor, Color backgroundColor, float size)
        {
            switch (inline)
            {
                case LiteralInline literal:
                    return new[]
                    {
                        new Span
                        {
                            Text = literal.Content.Text.Substring(literal.Content.Start, literal.Content.Length),
                            FontAttributes = attributes,
                            ForegroundColor = foregroundColor,
                            BackgroundColor = backgroundColor,
                            FontSize = size,
                            FontFamily = family,
                        }
                    };

                case EmphasisInline emphasis:
                    var childAttributes = attributes | (emphasis.DelimiterCount == 2 /*.IsDouble*/ ? FontAttributes.Bold : FontAttributes.Italic);
                    var espans = emphasis.SelectMany(x => CreateSpans(x, family, childAttributes, foregroundColor, backgroundColor, size));
                    return espans.ToArray();

                case LineBreakInline breakline:
                    return new[] { new Span { Text = "\n" } };

                case LinkInline link:

                    var url = link.Url;

                    if (!(url.StartsWith("http://") || url.StartsWith("https://")))
                    {
                        url = $"{this.RelativeUrlHost?.TrimEnd('/')}/{url.TrimStart('/')}";
                    }

                    if(link.IsImage)
                    {
                        var image = new Image();

                        if(Path.GetExtension(url) == ".svg")
                        {
                            image.RenderSvg(url);
                        }
                        else
                        {
                            image.Source = url;
                        }

                        //queuedViews.Add(image);
                        return new Span[0];
                    }
                    else
                    {
                        var spans = link.SelectMany(x => CreateSpans(x, this.Theme.Link.FontFamily ?? family, attributes| this.Theme.Link.Attributes, this.Theme.Link.ForegroundColor, this.Theme.Link.BackgroundColor, size)).ToArray();
                        var newlink = new KeyValuePair<string, string>(string.Join("", spans.Select(x => x.Text)), url);
                        foreach (var span in spans)
                        {
                            AttachLink(span, newlink);
                        }
                        links.Add(new KeyValuePair<string, string>(string.Join("",spans.Select(x => x.Text)), url));
                        return spans;
                    }

                case CodeInline code:
                    return new[]
                    {
                        new Span()
                        {
                            Text="\u2002",
                            FontSize = size,
                            FontFamily = this.Theme.Code.FontFamily,
                            ForegroundColor = this.Theme.Code.ForegroundColor,
                            BackgroundColor = this.Theme.Code.BackgroundColor
                        },
                        new Span
                        {
                            Text = code.Content,
                            FontAttributes = this.Theme.Code.Attributes,
                            FontSize = size,
                            FontFamily = this.Theme.Code.FontFamily,
                            ForegroundColor = this.Theme.Code.ForegroundColor,
                            BackgroundColor = this.Theme.Code.BackgroundColor
                        },
                        new Span()
                        {
                            Text="\u2002",
                            FontSize = size,
                            FontFamily = this.Theme.Code.FontFamily,
                            ForegroundColor = this.Theme.Code.ForegroundColor,
                            BackgroundColor = this.Theme.Code.BackgroundColor
                        },
                    };

                case HtmlInline html:
                    if (html.Tag == "<!--br-->" || html.Tag == "<br>")
                    {
                        return new[]
                        {
                            new Span()
                            {
                                Text="\r\n",
                                FontSize = size,
                                FontFamily = family,
                                ForegroundColor = foregroundColor,
                                BackgroundColor = backgroundColor
                            },
                        };
                    }
                    return new Span[0];

                default:
                    Debug.WriteLine($"Can't render {inline.GetType()} inlines.");
                    return new Span[0];
            }
        }

        #endregion
    }
}
