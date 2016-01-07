using Microsoft.VisualStudio.Text.Tagging;

namespace WaveDev.VisualRoslynQuoter
{
    internal class HighlightSyntaxWithSymbolTag : TextMarkerTag
    {
        public HighlightSyntaxWithSymbolTag()
             : base("MarkerFormatDefinition/HighlightSyntaxWithSymbolFormatDefinition")
        {

        }
    }
}
