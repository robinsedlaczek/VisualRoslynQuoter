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

    /// <summary>
    /// Interaction logic for VisualRoslynQuoterToolWindowControl.
    /// </summary>
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public partial class VisualRoslynQuoterToolWindowControl : UserControl, IWpfTextViewCreationListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualRoslynQuoterToolWindowControl"/> class.
        /// </summary>
        public VisualRoslynQuoterToolWindowControl()
        {
            this.InitializeComponent();
        }

        public void TextViewCreated(IWpfTextView textView)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "VisualRoslynQuoterToolWindow");
        }
    }
}