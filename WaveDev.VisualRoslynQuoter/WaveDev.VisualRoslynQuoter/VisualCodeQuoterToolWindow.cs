//------------------------------------------------------------------------------
// <copyright file="VisualCodeQuoterToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace WaveDev.VisualRoslynQuoter
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TextManager.Interop;
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("6021db60-3722-4fd8-a29b-8ae88c3a41e5")]
    public class VisualCodeQuoterToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualCodeQuoterToolWindow"/> class.
        /// </summary>
        public VisualCodeQuoterToolWindow() : base(null)
        {
            Caption = "Visual Code Quoter";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = new VisualCodeQuoterToolWindowControl();

            var codeWindow = GetService(typeof(SVsCodeWindow)) as IVsCodeWindow;

            if (codeWindow != null)
            {
                IVsTextLines textLines;
                var getBufferResult = codeWindow.GetBuffer(out textLines);
            }
        }


    }
}
