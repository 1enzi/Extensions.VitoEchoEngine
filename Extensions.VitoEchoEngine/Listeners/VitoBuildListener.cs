using Extensions.VitoEchoEngine.Utils;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System;

namespace Extensions.VitoEchoEngine.Listeners
{
    public class VitoBuildListener : IVsUpdateSolutionEvents
    {
        private readonly Action<string> _onBuildQuote;

        public VitoBuildListener(Action<string> onBuildQuote)
        {
            _onBuildQuote = onBuildQuote;
        }

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            string quote = fSucceeded == 1
                ? VitoQuoteEngine.GetBuildSuccessQuote()
                : VitoQuoteEngine.GetBuildFailureQuote();

            _onBuildQuote?.Invoke(quote);
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        public int UpdateProjectCfgs_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateProjectCfgs_Done(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }
    }
}
