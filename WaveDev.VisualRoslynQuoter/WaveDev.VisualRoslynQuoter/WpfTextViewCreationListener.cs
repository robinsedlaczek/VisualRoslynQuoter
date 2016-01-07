using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;
using System.IO;
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
                var sourceNode = tree.GetRoot() as CSharpSyntaxNode;

                var memberAccessExpressions = sourceNode.DescendantNodes().OfType<MemberAccessExpressionSyntax>();

                var file = typeof(object).Assembly.CodeBase.Substring(8);
                var reference = MetadataReference.CreateFromFile(file);
                var compilation = CSharpCompilation
                    .Create("CodeInCurrentView")
                    .AddReferences(reference)
                    .AddSyntaxTrees(tree);

                var semanticModel = compilation.GetSemanticModel(tree);

                var info =
                    from expression in memberAccessExpressions
                    select new
                    {
                        Syntax = expression.Expression,
                        SymbolInfo = semanticModel.GetSymbolInfo(expression.Expression),
                        TypeInfo = semanticModel.GetTypeInfo(expression.Expression)
                    };



                foreach (var expression in memberAccessExpressions)
                {
                    var symbolInfo = semanticModel.GetSymbolInfo(expression);
                    var typeInfo = semanticModel.GetTypeInfo(expression);

                }

                //var docComment = symbolInfo.Symbol.GetDocumentationCommentXml();

            }
            catch (Exception exception)
            {

            }
        }
    }
}
