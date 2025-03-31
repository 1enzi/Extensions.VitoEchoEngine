using System;
using System.Collections.Generic;
using Extensions.VitoEchoEngine.Models;
using Extensions.VitoEchoEngine.Models.Enum;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class DevModeOverlay
    {
        private static DevOverlayQuotes _devQuotes = new();
        private static bool _isEnable = false;

        public static void Init()
        {
            _devQuotes = EmbeddedQuoteLoader.LoadDevOverlayQuotes();
        }

        public static void ToggleIsEnable() => _isEnable = !_isEnable;
        public static bool IsEnable() => _isEnable;

        public static string GetEcho(VitoMood mood)
        {
            var list = _devQuotes.MoodQuotes.TryGetValue(mood.ToString(), out var moodList) ? moodList : null;
            var echo = GetRandom(moodList);

            var timeEcho = GetTimeBased();
            return timeEcho != null ? $"{echo}\n// {timeEcho}" : $"\n\n//{echo}";
        }

        private static string GetTimeBased()
        {
            var hour = DateTime.Now.Hour;
            var key = hour switch
            {
                >= 0 and < 6 => "Night",
                >= 6 and < 12 => "Morning",
                >= 18 and <= 23 => "Evening",
                _ => null
            };

            return key != null && _devQuotes.TimeBased.TryGetValue(key, out var list)
                ? GetRandom(list)
                : null;
        }

        private static string GetRandom(List<string> list)
        {
            if (list == null || list.Count == 0) return null;
            return list[new Random().Next(list.Count)];
        }
    }
}
