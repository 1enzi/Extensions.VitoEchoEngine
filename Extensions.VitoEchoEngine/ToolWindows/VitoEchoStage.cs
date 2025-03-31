using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace Extensions.VitoEchoEngine.ToolWindows
{
    [Guid("4E73E9D3-F816-4055-B44A-5853A4DB68D8")]
    public class VitoEchoStage : ToolWindowPane
    {
        public VitoEchoStage() : base(null)
        {
            Caption = "Vito's EchoStage";
            Content = new VitoEchoStageControl();
        }
    }
}
