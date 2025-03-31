using Extensions.VitoEchoEngine.Models.Enum;
using System;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class VitoQuoteEngine
    {
        public static string GetTodayQuote()
        {
            var day = DateTime.Now.DayOfWeek.ToString();
            return WithDevLayer(VitoQuoteStore.GetQuoteFor(day, VitoMoodMonitor.CurrentMood));
        }

        public static string GetBuildFailureQuote()
            => WithDevLayer(VitoQuoteStore.GetQuoteFor("BuildFailure", VitoMoodMonitor.CurrentMood));

        public static string GetBuildSuccessQuote()
            => WithDevLayer(VitoQuoteStore.GetQuoteFor("BuildSuccess", VitoMoodMonitor.CurrentMood));

        private static string WithDevLayer(string quote)
        {
            if (!DevModeOverlay.IsEnable())
                return quote;

            var devEcho = DevModeOverlay.GetEcho(VitoMoodMonitor.CurrentMood);
            return $"{quote}\n{devEcho}";
        }
    }
}
