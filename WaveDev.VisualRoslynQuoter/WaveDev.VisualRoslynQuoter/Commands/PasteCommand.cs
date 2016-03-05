using Microsoft.VisualStudio.Text;
using System;
using System.IO;
using System.Windows.Input;
using WaveDev.VisualRoslynQuoter.ViewModels;

namespace WaveDev.VisualRoslynQuoter.Commands
{
    internal class PasteCommand : ICommand
    {
        private QuoterViewModel _model;
        private ITextSnapshot _textSnapshot;

        public PasteCommand(QuoterViewModel model)
        {
            _model = model;
            _model.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_model.QuotedCode) && CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
            else if (e.PropertyName == nameof(_model.TextSnapshot))
                _textSnapshot = _model.TextSnapshot;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var thereIsQuotedCode =
                !string.IsNullOrEmpty(_model.QuotedCode) && !string.IsNullOrWhiteSpace(_model.QuotedCode);

            return thereIsQuotedCode;
        }

        public void Execute(object parameter)
        {
            if (_textSnapshot.TextBuffer.CheckEditAccess())
            {
                _textSnapshot.TextBuffer.Insert(0, _model.QuotedCode);
            }
        }
    }
}