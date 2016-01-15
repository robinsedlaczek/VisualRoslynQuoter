//------------------------------------------------------------------------------
// <copyright file="DTEEditorClassifierProvider.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using EnvDTE80;

namespace WaveDev.VisualRoslynQuoter
{
    /// <summary>
    /// Classifier provider. It adds the classifier to the set of classifiers.
    /// </summary>
    [Export(typeof(IClassifierProvider))]
    [ContentType("text")] // This classifier applies to all text files.
    internal class DTEEditorClassifierProvider : IClassifierProvider
    {
        // Disable "Field is never assigned to..." compiler's warning. Justification: the field is assigned by MEF.
#pragma warning disable 649

        /// <summary>
        /// Classification registry to be used for getting a reference
        /// to the custom classification type later.
        /// </summary>
        [Import]
        private IClassificationTypeRegistryService classificationRegistry;

        [Import]
        internal SVsServiceProvider ServiceProvider = null;

#pragma warning restore 649

        #region IClassifierProvider

        /// <summary>
        /// Gets a classifier for the given text buffer.
        /// </summary>
        /// <param name="buffer">The <see cref="ITextBuffer"/> to classify.</param>
        /// <returns>A classifier for the text buffer, or null if the provider cannot do so in its current state.</returns>
        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            var dte = (DTE)ServiceProvider.GetService(typeof(DTE));
            var dte2 = (DTE2)dte;

            var project = dte.Solution.Projects.Item(1);
            var codeElement = project.CodeModel.CodeElements.Item(1).FullName;

            return buffer.Properties.GetOrCreateSingletonProperty<DTEEditorClassifier>(creator: () => new DTEEditorClassifier(this.classificationRegistry));
        }

        #endregion
    }
}
