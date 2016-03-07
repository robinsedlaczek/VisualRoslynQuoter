using Microsoft.VisualStudio.Text;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WaveDev.VisualRoslynQuoter.ViewModels;

namespace WaveDev.VisualRoslynQuoter.Commands
{
    internal class CopyCommand : ICommand
    {
        #region Private Fields

        private QuoterViewModel _model;
        private ITextSnapshot _textSnapshot;

        #endregion

        #region Construction

        public CopyCommand(QuoterViewModel model)
        {
            _model = model;
            _model.PropertyChanged += OnModelPropertyChanged;
        }

        #endregion

        #region Public Members

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
            Clipboard.SetText(_model.QuotedCode, TextDataFormat.UnicodeText);
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