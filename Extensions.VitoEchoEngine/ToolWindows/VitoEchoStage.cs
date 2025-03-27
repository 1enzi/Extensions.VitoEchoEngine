using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace Extensions.VitoEchoEngine.ToolWindows
{
    [Guid("a1f7c438-5c45-4bde-8f11-02d1dcbf5617")]
    public class VitoEchoStage : ToolWindowPane
    {
        public VitoEchoStage() : base(null)
        {
            Caption = "Vito's EchoStage";
            Content = new VitoEchoStageControl();
        }
    }
}
