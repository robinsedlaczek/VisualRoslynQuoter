using Microsoft.VisualStudio.Text;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WaveDev.VisualRoslynQuoter.ViewModels;

namespace WaveDev.VisualRoslynQuoter.Commands
{
    internal class PasteCommand : ICommand
    {
        #region Private Fields

        private QuoterViewModel _model;
        private ITextSnapshot _textSnapshot;

        #endregion

        #region Construction

        public PasteCommand(QuoterViewModel model)
        {
            _model = model;
            _model.PropertyChanged += OnModelPropertyChanged;
        }

        #endregion

        #region ICommand

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var thereIsQuotedCode =
                !string.IsNullOrEmpty(_model.QuotedCode) && !string.IsNullOrWhiteSpace(_model.QuotedCode);

            return thereIsQuotedCode;
        }

        public void Execute(object parameter)
        {
            var operations = _model.EditorOperationsFactoryService.GetEditorOperations(_model.TextView);

            if (operations.CanPaste)
            {
                var oldTextInClipboard = Clipboard.ContainsText(TextDataFormat.UnicodeText) ? Clipboard.GetText(TextDataFormat.UnicodeText) : string.Empty;

                Clipboard.SetText(_model.QuotedCode, TextDataFormat.UnicodeText);
                operations.Paste();

                if (!string.IsNullOrEmpty(oldTextInClipboard))
                    Clipboard.SetText(oldTextInClipboard, TextDataFormat.UnicodeText);
            }
        }

        #endregion

        #region Private Members

        private void OnModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_model.QuotedCode):
                    break;
                case nameof(_model.TextSnapshot):
                    _textSnapshot = _model.TextSnapshot;
                    break;
                default:
                    return;
            }

            FireCanExecuteChanged();
        }

        private void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        #endregion
    }
}