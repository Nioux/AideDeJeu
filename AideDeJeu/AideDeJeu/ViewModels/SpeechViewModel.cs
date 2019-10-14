using AideDeJeuLib;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class SpeechViewModel : BaseViewModel
    {
        private Command<Item> _SpeakItemCommand = null;
        public Command<Item> SpeakItemCommand
        {
            get
            {
                return _SpeakItemCommand ?? (_SpeakItemCommand = new Command<Item>(async (item) => await ExecuteSpeakItemCommandAsync(item)));
            }
        }

        private Command _CancelSpeakCommand = null;
        public Command CancelSpeakCommand
        {
            get
            {
                return _CancelSpeakCommand ?? (_CancelSpeakCommand = new Command(() => ExecuteCancelSpeakCommand()));
            }
        }

        public string SpeakerIcon
        {
            get
            {
                return Speaking ? "speaker.png" : "speaker_off.png";
            }
        }
        public bool Speaking
        {
            get
            {
                return _CancellationTokenSource != null;
            }
        }
        public bool NotSpeaking
        {
            get
            {
                return _CancellationTokenSource == null;
            }
        }
        private CancellationTokenSource _CancellationTokenSource = null;
        public async Task ExecuteSpeakItemCommandAsync(Item item)
        {
            if (Speaking)
            {
                ExecuteCancelSpeakCommand();
                return;
            }
            var md = MarkdownToSpeakableString(item.Markdown);
            try
            {
                _CancellationTokenSource = new CancellationTokenSource();
                OnPropertyChanged(nameof(Speaking));
                OnPropertyChanged(nameof(NotSpeaking));
                OnPropertyChanged(nameof(SpeakerIcon));
                var options = new Xamarin.Essentials.SpeechOptions();
                var locales = (await Xamarin.Essentials.TextToSpeech.GetLocalesAsync()).ToList();
                if (item.Id.Contains("_vo.md"))
                {
                    options.Locale = locales.FirstOrDefault(l => l.Language.StartsWith("en"));
                }
                else
                {
                    options.Locale = locales.FirstOrDefault(l => l.Language.StartsWith("fr"));
                }
            
                await Xamarin.Essentials.TextToSpeech.SpeakAsync(md, options, _CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _CancellationTokenSource?.Dispose();
                _CancellationTokenSource = null;
                OnPropertyChanged(nameof(Speaking));
                OnPropertyChanged(nameof(NotSpeaking));
                OnPropertyChanged(nameof(SpeakerIcon));
            }
        }

        public void ExecuteCancelSpeakCommand()
        {
            _CancellationTokenSource?.Cancel();
        }

        public string MarkdownToSpeakableString(string md)
        {
            var pipeline = new Markdig.MarkdownPipelineBuilder().UseYamlFrontMatter().UsePipeTables().Build();
            var parsed = Markdig.Markdown.Parse(md, pipeline);
            var speakable = Render(parsed.AsEnumerable());
            return speakable;
        }
        private string Render(IEnumerable<Block> blocks)
        {
            var phrases = "";
            foreach (var block in blocks)
            {
                var phrase = this.Render(block);
                if (phrase != null)
                {
                    phrases += phrase;
                }
                if (block.IsBreakable)
                {
                    phrases += "\n";
                }
            }
            return phrases;
        }

        private string Render(Block block)
        {
            switch (block)
            {
                case HeadingBlock heading:
                    return Render(heading);

                case ParagraphBlock paragraph:
                    return Render(paragraph);

                //case QuoteBlock quote:
                //    Render(quote);
                //    break;

                //case CodeBlock code:
                //    Render(code);
                //    break;

                case ListBlock list:
                    return Render(list);
                //break;

                //case ThematicBreakBlock thematicBreak:
                //    Render(thematicBreak);
                //    break;

                //case HtmlBlock html:
                //    Render(html);
                //    break;

                case Markdig.Extensions.Tables.Table table:
                    return Render(table);
                //break;

                default:
                    Debug.WriteLine($"Can't render {block.GetType()} blocks.");
                    return null;
            }

            //if (queuedViews.Any())
            //{
            //    foreach (var view in queuedViews)
            //    {
            //        this.stack.Children.Add(view);
            //    }
            //    queuedViews.Clear();
            //}
        }

        private string Render(HeadingBlock block)
        {
            return CreateFormatted(block.Inline);
        }

        private string Render(ParagraphBlock block)
        {
            return CreateFormatted(block.Inline);
        }

        private string Render(ListBlock block)
        {
            //listScope++;

            var views = "";
            for (int i = 0; i < block.Count(); i++)
            {
                var item = block.ElementAt(i);

                if (item is ListItemBlock itemBlock)
                {
                    var subviews = Render(itemBlock);
                    if (subviews != null)
                    {
                        views += subviews;
                    }
                }
            }

            //listScope--;
            return views;
        }

        private string Render(ListItemBlock block)
        {
            var stack = "";

            var subv = this.Render(block.AsEnumerable());
            subv.ToList().ForEach(v => stack += v);

            return stack;
        }

        private string Render(Markdig.Extensions.Tables.Table tableBlock)
        {
            int maxColumns = 0;
            foreach (Markdig.Extensions.Tables.TableRow row in tableBlock)
            {
                maxColumns = Math.Max(maxColumns, row.Count);
            }

            var table = "";
            foreach (Markdig.Extensions.Tables.TableRow row in tableBlock)
            {
                foreach (Markdig.Extensions.Tables.TableCell cell in row)
                {
                    var phrase = "";
                    foreach (var blockpar in cell)
                    {
                        var par = blockpar as Markdig.Syntax.ParagraphBlock;
                        var blockPhrase = CreateFormatted(par.Inline);
                        phrase += blockPhrase + " ";
                    }
                    table += phrase + " ";
                }
                table += "\n";
            }

            return table;
        }



        private string CreateFormatted(ContainerInline inlines)
        {
            var phrase = "";
            foreach (var inline in inlines)
            {
                var spans = CreateChunks(inline);
                if (spans != null)
                {
                    foreach (var span in spans)
                    {
                        phrase += span;
                    }
                }
            }
            return phrase;
        }

        private IEnumerable<string> CreateChunks(Inline inline)
        {
            switch (inline)
            {
                case LiteralInline literal:
                    return new string[]
                    {
                        literal.Content.Text.Substring(literal.Content.Start, literal.Content.Length)
                    };
                case EmphasisInline emphasis:
                    var espans = emphasis.SelectMany(x => CreateChunks(x));
                    return espans.ToArray();

                case LineBreakInline breakline:
                    return new string[] { "\n" };

                case LinkInline link:
                    return link.SelectMany(x => CreateChunks(x));

                case CodeInline code:

                case HtmlInline html:

                default:
                    return new string[] { };
            }


        }
    }
}