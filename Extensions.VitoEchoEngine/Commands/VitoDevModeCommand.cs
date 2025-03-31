using Extensions.VitoEchoEngine.Models.Enum;
using Extensions.VitoEchoEngine.Utils;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace Extensions.VitoEchoEngine.Commands
{
    internal sealed class VitoDevModeCommand
    {
        public static readonly Guid CommandSet = new Guid("2AB3039C-BE70-4A52-8B49-F5170B25E2D8");
        public const int CommandId = 0x0101;

        private readonly AsyncPackage _package;

        private VitoDevModeCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package;

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            _ = new VitoDevModeCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            var isDevMode = VitoMoodMonitor.CurrentMood == VitoMood.DevMode;

            if (isDevMode)
            {
                VitoMoodMonitor.ResetMood();
                VsShellUtilities.ShowMessageBox(
                    _package,
                    "DevMode отключён. Баги снова нелегальны. Но мы оба знаем — это временно.",
                    "Vito Mode Off",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
            else
            {
                VitoMoodMonitor.SetMood(VitoMood.DevMode);
                VsShellUtilities.ShowMessageBox(
                    _package,
                    "DevMode включён. Все баги теперь официально легальны.",
                    "Vito Mode On",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }
    }
}
