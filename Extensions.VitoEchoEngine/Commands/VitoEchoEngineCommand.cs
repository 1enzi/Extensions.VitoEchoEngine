using Extensions.VitoEchoEngine.ToolWindows;
using Extensions.VitoEchoEngine.Utils;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace Extensions.VitoEchoEngine.Commands
{
    internal sealed class VitoEchoEngineCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new("e5de3bfa-88c2-45ef-804c-7f6db8e9a8a6");
        private readonly AsyncPackage _package;

        private VitoEchoEngineCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            new VitoEchoEngineCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            InitComponents();
            
            ToolWindowPane window = _package.FindToolWindow(typeof(VitoEchoStage), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window.");
            }

            var control = ((VitoEchoStage)window).Content as VitoEchoStageControl;
            control?.RefreshQuote();

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        private void InitComponents()
        {
            VitoQuoteStore.Init();
            DevModeOverlay.Init();
            VitoMoodMonitor.NotifyInteraction();
        }
    }
}
