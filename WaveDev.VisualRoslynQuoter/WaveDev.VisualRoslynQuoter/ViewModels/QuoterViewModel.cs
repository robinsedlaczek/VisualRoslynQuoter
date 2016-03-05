﻿using Microsoft.VisualStudio.Text.Editor;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.VisualStudio.Text;
using System.Runtime.CompilerServices;
using WaveDev.VisualRoslynQuoter.Commands;
using Microsoft.VisualStudio.Text.Operations;

namespace WaveDev.VisualRoslynQuoter.ViewModels
{
    public class QuoterViewModel : INotifyPropertyChanged
    {
        private string _quotedCode;
        private string _hintText;
        private ITextSnapshot _textSnapshot;

        public QuoterViewModel()
        {
            WpfTextViewCreationListener.TextViewLayoutChanged += OnWpfTextViewCreationListenerTextViewLayoutChanged;

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
                return "Copy the quoted code from this window into the clipboard.";
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
                return "Paste the quoted code from this window into the current code editor at the current cursor position.";
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

        private void OnWpfTextViewCreationListenerTextViewLayoutChanged(TextViewLayoutChangedEventArgs e, ITextView textView, IEditorOperationsFactoryService editorOperationsFactoryService)
        {
            try
            {
                TextView = textView;
                TextSnapshot = e.NewSnapshot;
                EditorOperationsFactoryService = editorOperationsFactoryService;

                var code = TextSnapshot.GetText();

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
