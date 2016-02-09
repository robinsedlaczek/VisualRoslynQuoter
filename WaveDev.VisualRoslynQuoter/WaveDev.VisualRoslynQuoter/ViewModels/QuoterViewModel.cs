using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel;

namespace WaveDev.VisualRoslynQuoter.ViewModels
{
    public class QuoterViewModel : INotifyPropertyChanged
    {
        private string _quotedCode;
        private string _hintText;

        public QuoterViewModel()
        {
            WpfTextViewCreationListener.TextViewLayoutChanged += OnWpfTextViewCreationListenerTextViewLayoutChanged;

            HintText = "No Code Selected";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string OldCode
        {
            get;
            set;
        }

        public string QuotedCode
        {
            get
            {
                return _quotedCode;
            }

            private set
            {
                if (_quotedCode != value)
                {
                    _quotedCode = value;

                    NotifyPropertyChanged(nameof(QuotedCode));

                    if (!string.IsNullOrEmpty(_quotedCode.Trim()))
                        HintText = "Use the following Roslyn Syntax Api calls to generate the selected code from the active editor window.";
                    else
                        HintText = "No Code Selected";
                }
            }
        }

        public string HintText
        {
            get
            {
                return _hintText;
            }

            set
            {
                if (_hintText != value)
                {
                    _hintText = value;

                    NotifyPropertyChanged(nameof(HintText));
                }
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnWpfTextViewCreationListenerTextViewLayoutChanged(TextViewLayoutChangedEventArgs e)
        {
            try
            {
                var code = e.NewSnapshot.GetText();

                if (code == OldCode)
                    return;

                var quoter = new Quoter();

                QuotedCode = quoter.Quote(code);
                OldCode = code;
            }
            catch (Exception exception)
            {

            }
        }
    }
}
