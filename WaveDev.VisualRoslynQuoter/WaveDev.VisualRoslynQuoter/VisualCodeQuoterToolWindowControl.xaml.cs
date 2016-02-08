//------------------------------------------------------------------------------
// <copyright file="VisualCodeQuoterToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace WaveDev.VisualRoslynQuoter
{
    using Microsoft.VisualStudio.Text.Editor;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for VisualCodeQuoterToolWindowControl.
    /// </summary>
    public partial class VisualCodeQuoterToolWindowControl : UserControl
    {
        public string OldCode{ get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualCodeQuoterToolWindowControl"/> class.
        /// </summary>
        public VisualCodeQuoterToolWindowControl()
        {
            this.InitializeComponent();

            WpfTextViewCreationListener.TextViewLayoutChanged += OnWpfTextViewCreationListenerTextViewLayoutChanged;
        }

        private void OnWpfTextViewCreationListenerTextViewLayoutChanged(TextViewLayoutChangedEventArgs e)
        {
            try
            {
                var code = e.NewSnapshot.GetText();

                if (code == OldCode)
                    return;

                var quoter = new Quoter();
                var quotedCode = quoter.Quote(code);

                QuotedCodeTextBox.Text = quotedCode;
                OldCode = code;
            }
            catch (Exception exception)
            {

            }
        }
    }
}