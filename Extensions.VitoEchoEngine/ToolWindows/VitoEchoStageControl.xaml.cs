using System.Windows.Forms;

namespace Extensions.VitoEchoEngine.ToolWindows
{
    public partial class VitoEchoStageControl : UserControl
    {
        public VitoEchoStageControl()
        {
            InitializeComponent();
            QuoteTextBlock.Text = VitoEchoEngine.GetTodayQuote();
        }
    }
}
