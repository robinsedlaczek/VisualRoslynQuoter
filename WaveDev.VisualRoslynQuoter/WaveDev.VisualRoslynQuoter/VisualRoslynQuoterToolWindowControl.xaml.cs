//------------------------------------------------------------------------------
// <copyright file="VisualRoslynQuoterToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace WaveDev.VisualRoslynQuoter
{
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    /// <summary>
    /// Interaction logic for VisualRoslynQuoterToolWindowControl.
    /// </summary>
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public partial class VisualRoslynQuoterToolWindowControl : UserControl, IWpfTextViewCreationListener
    {
        private IWpfTextView _textView;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualRoslynQuoterToolWindowControl"/> class.
        /// </summary>
        public VisualRoslynQuoterToolWindowControl()
        {
            this.InitializeComponent();
        }

        public void TextViewCreated(IWpfTextView textView)
        {
            _textView = textView;
            _textView.LayoutChanged += OnTextViewLayoutChanged;
        }

        private void OnTextViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            try
            {
                var code = @"
                    using System;
                    using System.Collections.Generic;
                    using System.Linq;
                    using System.Text;
                    using System.Threading.Tasks;

                    namespace ConsoleApplication1
                    {
                        class Program
                        {
                            static void Main(string[] args)
                            {
                                DoSomething();
                            }

                            public static void DoSomething()
                            {

                            }
                        }
                    }
                    ";

                //var code = e.NewSnapshot.GetText();
                var sourceNode = CSharpSyntaxTree.ParseText(code).GetRoot() as CSharpSyntaxNode;

                var quoter = new Quoter
                {
                    OpenParenthesisOnNewLine = false,
                    ClosingParenthesisOnNewLine = false,
                    UseDefaultFormatting = false,
                    RemoveRedundantModifyingCalls = false
                };

                var generatedCode = quoter.Quote(sourceNode);

                SyntaxFactoryCodeTextBox.Text = generatedCode;
            }
            catch (Exception exception)
            {
                SyntaxFactoryCodeTextBox.Text = exception.Message;
            }
        }
    }
}