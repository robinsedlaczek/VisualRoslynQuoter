using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace WaveDev.VisualRoslynQuoter
{
    internal class SemanticInfo
    {
        public SyntaxKind Kind { get; set; }
        public SymbolInfo SymbolInfo { get; set; }
        public SyntaxNode Syntax { get; set; }
        public TypeInfo TypeInfo { get; set; }
    }
}