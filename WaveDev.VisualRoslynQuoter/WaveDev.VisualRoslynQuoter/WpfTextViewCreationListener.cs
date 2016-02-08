using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace WaveDev.VisualRoslynQuoter
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class WpfTextViewCreationListener : IWpfTextViewCreationListener
    {
        private IWpfTextView _textView;

        public void TextViewCreated(IWpfTextView textView)
        {
            _textView = textView;
            _textView.LayoutChanged += OnTextViewLayoutChanged;
        }

        private void OnTextViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            try
            {
                var code = e.NewSnapshot.GetText();
                var tree = CSharpSyntaxTree.ParseText(code);
                var sourceNode = tree.GetRoot() as CompilationUnitSyntax;

                var currentDocument = e.NewSnapshot.GetOpenDocumentInCurrentContextWithChanges();

                var memberAccessExpressions = sourceNode.DescendantNodes().OfType<MemberAccessExpressionSyntax>();

                var file = typeof(object).Assembly.CodeBase.Substring(8);
                var reference = MetadataReference.CreateFromFile(file);
                var compilation = CSharpCompilation
                    .Create("CodeInCurrentView")
                    .AddReferences(reference)
                    .AddSyntaxTrees(tree);

                var semanticModel = compilation.GetSemanticModel(tree);

                var symbolInfos = new List<SemanticInfo>();
                var typeInfos = new List<SemanticInfo>();
                var symbolAndTypeInfos = new List<SemanticInfo>();

                foreach (var node in sourceNode.DescendantNodes())
                {
                    var targetList = symbolAndTypeInfos;
                    var symbolInfo = semanticModel.GetSymbolInfo(node);
                    var typeInfo = semanticModel.GetTypeInfo(node);

                    if (symbolInfo.Symbol != null)
                    {
                        var displayString = string.Empty;
                        INamedTypeSymbol namedType = symbolInfo.Symbol.ContainingType;

                        if (namedType != null)
                            displayString = namedType.ToDisplayString();
                    }

                    if (symbolInfo.Symbol != null && typeInfo.Type == null)
                        targetList = symbolInfos;
                    else if (symbolInfo.Symbol == null && typeInfo.Type != null)
                        targetList = typeInfos;
                    else if (symbolInfo.Symbol == null && typeInfo.Type == null)
                        continue;

                    targetList.Add(
                        new SemanticInfo
                        {
                            Syntax = node,
                            Kind = node.Kind(),
                            SymbolInfo = symbolInfo,
                            TypeInfo = typeInfo
                        });
                }


                //var docComment = symbolInfo.Symbol.GetDocumentationCommentXml();

            }
            catch (Exception exception)
            {

            }
        }
    }
}
