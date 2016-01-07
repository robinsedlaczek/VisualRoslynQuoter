//------------------------------------------------------------------------------
// <copyright file="VisualRoslynQuoterToolWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace WaveDev.VisualRoslynQuoter
{
    using System.Windows;
    using System.Windows.Controls;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// Interaction logic for VisualRoslynQuoterToolWindowControl.
    /// </summary>
    public partial class VisualRoslynQuoterToolWindowControl : UserControl, INotifyPropertyChanged
    {
        private string _quoterText;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualRoslynQuoterToolWindowControl"/> class.
        /// </summary>
        public VisualRoslynQuoterToolWindowControl()
        {
            this.InitializeComponent();

            DataContext = this;
        }

        public string QuoterText
        {
            get
            {
                var hash = GetHashCode();
                Trace.WriteLine("[ToolWindow] hash code: " + hash);

                return _quoterText;
            }

            set
            {
                if (_quoterText != value)
                {
                    _quoterText = value;

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("QuoterText"));
                }
            }
        }

        private void SetText(string text)
        {
            SyntaxFactoryCodeTextBox.Text = text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => SyntaxFactoryCodeTextBox.Text = "blubber");
        }
    }
}