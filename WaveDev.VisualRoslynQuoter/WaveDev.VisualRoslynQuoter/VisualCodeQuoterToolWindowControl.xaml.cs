//------------------------------------------------------------------------------
// <copyright file="VisualCodeQuoterToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace WaveDev.VisualRoslynQuoter
{
    using System.Windows.Controls;
    using ViewModels;
    /// <summary>
    /// Interaction logic for VisualCodeQuoterToolWindowControl.
    /// </summary>
    public partial class VisualCodeQuoterToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualCodeQuoterToolWindowControl"/> class.
        /// </summary>
        public VisualCodeQuoterToolWindowControl()
        {
            InitializeComponent();

            DataContext = new QuoterViewModel();
        }

    }
}