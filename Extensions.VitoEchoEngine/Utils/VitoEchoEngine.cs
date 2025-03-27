using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class VitoEchoEngine
    {
        public static string GetTodayQuote()
        {
            var day = DateTime.Now.DayOfWeek.ToString();
            return VitoQuoteStore.GetQuoteFor(day);
        }

        public static string GetBuildFailureQuote() => VitoQuoteStore.GetQuoteFor("BuildFailure");
        public static string GetBuildSuccessQuote() => VitoQuoteStore.GetQuoteFor("BuildSuccess");
    }
}
