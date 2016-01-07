using Microsoft.VisualStudio.Shell.Interop;

namespace WaveDev.VisualRoslynQuoter
{
    internal static class Services
    {
        public static IVsSolution VsSolution { get; set; }
    }
}
