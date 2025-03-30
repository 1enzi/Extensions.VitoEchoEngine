using System.Windows.Controls;
using Extensions.VitoEchoEngine.Utils;

namespace Extensions.VitoEchoEngine.ToolWindows
{
    public partial class VitoEchoStageControl : UserControl
    {
        public VitoEchoStageControl()
        {
            InitializeComponent();
            QuoteTextBlock.Text = VitoQuoteEngine.GetTodayQuote();
        }

        public void DisplayQuote(string quote)
        {
            QuoteTextBlock.Text = quote;
        }

        public void RefreshQuote()
        {
            QuoteTextBlock.Text = VitoQuoteEngine.GetTodayQuote();
        }
    }
}
