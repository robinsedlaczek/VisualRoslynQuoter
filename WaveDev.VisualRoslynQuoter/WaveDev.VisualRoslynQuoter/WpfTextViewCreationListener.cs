using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace WaveDev.VisualRoslynQuoter
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class WpfTextViewCreationListener : IWpfTextViewCreationListener
    {
        public delegate void TextViewLayoutChangedEventHanlder(TextViewLayoutChangedEventArgs e);
        public static event TextViewLayoutChangedEventHanlder TextViewLayoutChanged;

        private IWpfTextView _textView;

        public void TextViewCreated(IWpfTextView textView)
        {
            _textView = textView;
            _textView.LayoutChanged += OnTextViewLayoutChanged;
        }

        private void OnTextViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            try
            {
                if (TextViewLayoutChanged != null)
                    TextViewLayoutChanged(e);
            }
            catch (Exception exception)
            {

            }
        }
    }
}
