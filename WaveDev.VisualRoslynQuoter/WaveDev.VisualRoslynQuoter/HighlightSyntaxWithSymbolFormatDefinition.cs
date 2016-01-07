using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace WaveDev.VisualRoslynQuoter
{
    [Export(typeof(EditorFormatDefinition))]
    [Name("MarkerFormatDefinition/HighlightSyntaxWithSymbolFormatDefinition")]
    [UserVisible(true)]
    internal class HighlightSyntaxWithSymbolFormatDefinition : MarkerFormatDefinition
    {
        public HighlightSyntaxWithSymbolFormatDefinition()
        {
            BackgroundColor = Colors.LightBlue;
            ForegroundColor = Colors.DarkBlue;
            DisplayName = "Highlight Syntax with Symbol";
            ZOrder = 5;
        }
    }
}
