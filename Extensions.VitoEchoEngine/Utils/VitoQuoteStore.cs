﻿using Extensions.VitoEchoEngine.Models;
using Extensions.VitoEchoEngine.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class VitoQuoteStore
    {
        private static Dictionary<string, Dictionary<VitoMood, List<string>>> _quotes;
        private static DevOverlayQuotes _devOverlayQuotes;

        public static void Init()
        {
            _quotes = EmbeddedQuoteLoader.LoadMainQuotes();
            _devOverlayQuotes = EmbeddedQuoteLoader.LoadDevOverlayQuotes();
        }

        public static string GetQuoteFor(string key, VitoMood mood = VitoMood.Normal)
        {
            if (_quotes.TryGetValue(key, out var moodDict))
            {
                if (moodDict.TryGetValue(mood, out var moodQuotes) && moodQuotes.Any())
                    return GetRandom(moodQuotes);

                if (moodDict.TryGetValue(VitoMood.Normal, out var normalQuotes))
                    return GetRandom(normalQuotes);
            }

            return "[v~\\∞ ^•]… тишина.";
        }

        public static string GetRandomOverlayQuote()
        {
            if (_devOverlayQuotes == null) 
                return string.Empty;

            var all = _devOverlayQuotes.TimeBased.Values
                            .Concat(_devOverlayQuotes.Other.Values)
                            .SelectMany(q => q)
                            .ToList();

            return all.Count > 0 ? GetRandom(all) : string.Empty;
        }

        private static string GetRandom(List<string> list)
        {
            var rnd = new Random();
            return list[rnd.Next(list.Count)];
        }
    }
}
