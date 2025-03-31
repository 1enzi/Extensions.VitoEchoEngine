using Extensions.VitoEchoEngine.Listeners;
using Extensions.VitoEchoEngine.ToolWindows;
using Extensions.VitoEchoEngine.Utils;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using Timer = System.Timers.Timer;

namespace Extensions.VitoEchoEngine.Commands
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideMenuResource("VitoMenus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    [ProvideToolWindow(typeof(VitoEchoStage), Width = 200, Height = 150)]
    public sealed class VitoEchoEngineCommandPackage : AsyncPackage
    {
        /// <summary>
        /// VitoEchoEngineCommandPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "0962c31d-6a0c-4112-8fe8-6638124fb1dc";
        private Timer _moodTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="VitoEchoEngineCommandPackage"/> class.
        /// </summary>
        public VitoEchoEngineCommandPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await VitoEchoEngineCommand.InitializeAsync(this);
            await VitoDevModeCommand.InitializeAsync(this);

            if (await GetServiceAsync(typeof(SVsSolutionBuildManager)) is IVsSolutionBuildManager2 buildManager)
            {
                var listener = new VitoBuildListener(ShowBuildQuote);
                buildManager.AdviseUpdateSolutionEvents(listener, out _);
            }

            _moodTimer = new Timer
            {
                Interval = TimeSpan.FromMinutes(5).TotalMilliseconds,
                AutoReset = true,
                Enabled = true
            };
            _moodTimer.Elapsed += (_, _) => VitoMoodMonitor.Tick();
        }

        private void ShowBuildQuote(string quote)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (quote.Contains("ошибк") || quote.Contains("Exception") || quote.Contains("провал"))
                VitoMoodMonitor.NotifyBuildFailure();
            else
                VitoMoodMonitor.NotifyBuildSuccess();

            ToolWindowPane window = FindToolWindow(typeof(VitoEchoStage), 1, true);
            if (window?.Frame == null) 
                return;

            var control = (VitoEchoStage)window;
            if (control.Content is VitoEchoStageControl vitoControl)
            {
                vitoControl.DisplayQuote(quote);
            }
        }

        #endregion
    }
}
