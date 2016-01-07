﻿using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Operations;

namespace WaveDev.VisualRoslynQuoter
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(TextMarkerTag))]
    internal class HighlightSyntaxWithSymbolTaggerProvider : IViewTaggerProvider
    {
        [Import]
        internal ITextSearchService TextSearchService { get; set; }

        [Import]
        internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            
            //provide highlighting only on the top buffer 
            if (textView.TextBuffer != buffer)
                return null;

            ITextStructureNavigator textStructureNavigator =
                TextStructureNavigatorSelector.GetTextStructureNavigator(buffer);

            return new HighlightSyntaxWithSymbolTagger(textView, buffer, TextSearchService, textStructureNavigator) as ITagger<T>;
        }
    }
}
