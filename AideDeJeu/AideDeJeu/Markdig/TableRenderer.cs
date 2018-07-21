// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System.Globalization;
using AideDeJeu.Tools;
using Markdig.Syntax;

namespace Markdig.Renderers.Normalize
{
    /// <summary>
    /// An Normalize renderer for a <see cref="TableBlock"/>.
    /// </summary>
    /// <seealso cref="Markdig.Renderers.Normalize.NormalizeObjectRenderer{Markdig.Extensions.Tables.Table}" />
    public class TableRenderer : NormalizeObjectRenderer<Markdig.Extensions.Tables.Table>
    {
        protected override void Write(NormalizeRenderer renderer, Markdig.Extensions.Tables.Table obj)
        {
            foreach (Markdig.Extensions.Tables.TableRow row in obj)
            {
                renderer.Write("|");
                foreach (Markdig.Extensions.Tables.TableCell cell in row)
                {
                    foreach (Markdig.Syntax.ParagraphBlock block in cell)
                    {
                        renderer.Write(block.ToMarkdownString().Replace("\n", ""));
                    }
                    renderer.Write("|");
                }
                if (row.IsHeader)
                {
                    renderer.Write("\n|");
                    for (int i = 0; i < row.Count; i++)
                    {
                        renderer.Write("---|");
                    }
                }
                renderer.Write("\n");
            }

            //renderer.Write("\n\n" + obj.ToMarkdownString() + "\n\n");
            //var headingText = obj.Level > 0 && obj.Level <= 6
            //    ? HeadingTexts[obj.Level - 1]
            //    : new string('#', obj.Level);

            //renderer.Write(headingText).Write(' ');
            //renderer.WriteLeafInline(obj);

            //renderer.FinishBlock(renderer.Options.EmptyLineAfterHeading);
        }
    }
}