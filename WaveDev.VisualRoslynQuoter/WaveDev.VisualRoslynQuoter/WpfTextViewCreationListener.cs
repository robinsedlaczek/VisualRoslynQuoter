using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
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
        private IWpfTextView _textView;

        public delegate void TextViewLayoutChangedEventHanlder(TextViewLayoutChangedEventArgs e, ITextView textView, IEditorOperationsFactoryService editorOperationsFactoryService);
        public static event TextViewLayoutChangedEventHanlder TextViewLayoutChanged;

        [Import]
        public IEditorOperationsFactoryService EditorOperationsFactoryService;

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
                    TextViewLayoutChanged(e, _textView, EditorOperationsFactoryService);
            }
            catch (Exception exception)
            {

            }
        }
    }
}
