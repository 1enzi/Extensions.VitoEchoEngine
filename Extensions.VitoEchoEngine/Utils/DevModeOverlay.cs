using System;
using System.Collections.Generic;
using Extensions.VitoEchoEngine.Models;
using Extensions.VitoEchoEngine.Models.Enum;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class DevModeOverlay
    {
        private static readonly Dictionary<string, List<string>> _moodQuotes = new();
        private static readonly Dictionary<string, List<string>> _timeQuotes = new();

        public static void Load(DevOverlayQuotes quotes)
        {
            foreach (var kvp in quotes.TimeBased)
                _timeQuotes[kvp.Key] = kvp.Value;

            foreach (var kvp in quotes.Other)
                _moodQuotes[kvp.Key] = kvp.Value;
        }

        public static string GetEcho(VitoMood mood)
        {
            var list = _moodQuotes.TryGetValue(mood.ToString(), out var moodList) ? moodList : null;
            var echo = GetRandom(moodList);

            var timeEcho = GetTimeBased();
            return timeEcho != null ? $"{echo}\n// {timeEcho}" : echo;
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

            return key != null && _timeQuotes.TryGetValue(key, out var list)
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
