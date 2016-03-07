using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.VisualStudio.Text;
using System.Runtime.CompilerServices;
using WaveDev.VisualRoslynQuoter.Commands;
using Microsoft.VisualStudio.Text.Operations;
using System.Diagnostics;
using System.Text;

namespace WaveDev.VisualRoslynQuoter.ViewModels
{
    public class QuoterViewModel : INotifyPropertyChanged
    {
        private string _quotedCode;
        private string _hintText;
        private ITextSnapshot _textSnapshot;
        private QuotedCodeStyle _quotedCodeStyle;

        public QuoterViewModel()
        {
            WpfTextViewCreationListener.TextViewLayoutChanged += OnTextViewLayoutChanged;

            PasteCommand = new PasteCommand(this);
            CopyCommand = new CopyCommand(this);

            HintText = "No Code Selected";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CopyCommand
        {
            get;
            private set;
        }

        public string CopyCommandDescription
        {
            get
            {
                return "Copy the syntax generation code into the clipboard.";
            }
        }

        public string CopyCommandText
        {
            get
            {
                return "Copy to Clipboard";
            }
        }

        public ICommand PasteCommand
        {
            get;
            private set;
        }

        public string PasteCommandDescription
        {
            get
            {
                return "Paste the syntax generation code into your code.";
            }
        }

        public string PasteCommandText
        {
            get
            {
                return "Paste into Editor";
            }
        }

        public ITextSnapshot TextSnapshot
        {
            get
            {
                return _textSnapshot;
            }

            private set
            {
                if (_textSnapshot!=value)
                {
                    _textSnapshot = value;

                    NotifyPropertyChanged();
                }
            }
        }

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

                    NotifyPropertyChanged();

                    if (!string.IsNullOrEmpty(_quotedCode.Trim()))
                        HintText = "Use the following Roslyn Syntax Api calls to generate the selected code from the active editor window.";
                    else
                        HintText = "No Code Selected";
                }
            }
        }

        public QuotedCodeStyle QuotedCodeStyle
        {
            get
            {
                return _quotedCodeStyle;
            }

            private set
            {
                if (_quotedCodeStyle != value)
                {
                    _quotedCodeStyle = value;

                    NotifyPropertyChanged();
                    UpdateQuotedCode(true);
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

        public ITextView TextView
        {
            get;
            private set;
        }

        public IEditorOperationsFactoryService EditorOperationsFactoryService
        {
            get;
            private set;
        }

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnTextViewLayoutChanged(TextViewLayoutChangedEventArgs e, ITextView textView, IEditorOperationsFactoryService editorOperationsFactoryService)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                if (TextSnapshot == e.NewSnapshot)
                    return;

                TextView = textView;
                TextSnapshot = e.NewSnapshot;
                EditorOperationsFactoryService = editorOperationsFactoryService;

                UpdateQuotedCode(false);
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine(stopwatch.ElapsedMilliseconds + "ms for quoting code.");
            }
        }

        private void UpdateQuotedCode(bool forceUpdate)
        {
            try
            {
                var code = TextSnapshot.GetText();

                if (code == OldCode && !forceUpdate)
                    return;

                var quoter = new Quoter();
                var quotedCode = quoter.Quote(code);

                switch (QuotedCodeStyle)
                {
                    case QuotedCodeStyle.None:
                        break;
                    case QuotedCodeStyle.AsVariable:
                        quotedCode = $"var syntax = {quotedCode};";
                        break;
                    case QuotedCodeStyle.AsMethod:
                        quotedCode = $"private SyntaxNode GetSyntax()\r\n{{\r\nvar syntax = {quotedCode};\r\n\r\n\treturn syntax;\r\n}}";
                        break;
                    default:
                        break;
                }

                QuotedCode = quotedCode;
                OldCode = code;
            }
            catch (Exception exception)
            {

                throw;
            }
        }
    }
}
