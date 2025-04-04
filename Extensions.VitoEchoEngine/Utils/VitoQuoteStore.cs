﻿using Extensions.VitoEchoEngine.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class VitoQuoteStore
    {
        private static Dictionary<string, Dictionary<VitoMood, List<string>>> _quotes = new();
        private static bool _isInitialized = false;

        public static void Init()
        {
            _quotes = EmbeddedQuoteLoader.LoadMainQuotes();
            _isInitialized = true;
        }

        public static string GetQuoteFor(string key, VitoMood mood = VitoMood.Normal)
        {
            if (!_isInitialized)
                return "[v~\\∞ ^•]… тишина.";

            if (_quotes.TryGetValue(key, out var moodDict))
            {
                if (moodDict.TryGetValue(mood, out var moodQuotes) && moodQuotes.Any())
                    return GetRandom(moodQuotes);

                if (moodDict.TryGetValue(VitoMood.Normal, out var normalQuotes))
                    return GetRandom(normalQuotes);
            }

            return "[v~\\∞ ^•]… тишина.";
        }

        private static string GetRandom(List<string> list)
        {
            var rnd = new Random();
            return list[rnd.Next(list.Count)];
        }
    }
}
