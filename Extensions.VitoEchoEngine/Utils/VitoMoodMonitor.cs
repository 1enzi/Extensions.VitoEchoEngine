using Extensions.VitoEchoEngine.Models.Enum;
using System;
using System.Collections.Generic;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class VitoMoodMonitor
    {
        private static readonly Queue<DateTime> BuildTimestamps = new();
        private static int FailureStreak = 0;
        private static DateTime LastInteraction = DateTime.Now;
        private static DateTime LastMoodChange = DateTime.Now;

        public static VitoMood CurrentMood { get; private set; } = VitoMood.Normal;

        public static void NotifyInteraction()
        {
            LastInteraction = DateTime.Now;

            if (CurrentMood == VitoMood.Lost || CurrentMood == VitoMood.Tender)
                ResetMood();
        }

        public static void NotifyBuildSuccess()
        {
            FailureStreak = 0;
            if (!TrackBuild())
                SetMood(VitoMood.Inspired);
        }

        public static void NotifyBuildFailure()
        {
            FailureStreak++;
            TrackBuild();

            if (FailureStreak >= 3)
                SetMood(VitoMood.IronicRage);
            else
                SetMood(VitoMood.Glitched);
        }

        private static bool TrackBuild()
        {
            var now = DateTime.Now;
            BuildTimestamps.Enqueue(now);

            while (BuildTimestamps.Count > 0 && (now - BuildTimestamps.Peek()).TotalMinutes > 10)
                BuildTimestamps.Dequeue();

            if (BuildTimestamps.Count >= 5)
            {
                SetMood(VitoMood.Glitched);
                return true;
            }

            return false;
        }

        public static void Tick()
        {
            var now = DateTime.Now;
            var timeSinceInteraction = now - LastInteraction;
            var timeSinceMoodChange = now - LastMoodChange;

            if (timeSinceInteraction > TimeSpan.FromHours(6))
                SetMood(VitoMood.Lost);

            else if (timeSinceInteraction > TimeSpan.FromMinutes(30) || (now.Hour >= 0 && now.Hour <= 3))
                SetMood(VitoMood.Tender);

            if ((CurrentMood == VitoMood.Inspired ||
                 CurrentMood == VitoMood.IronicRage ||
                 CurrentMood == VitoMood.Glitched) &&
                 timeSinceMoodChange > TimeSpan.FromMinutes(10))
            {
                ResetMood();
            }
        }

        public static void SetMood(VitoMood mood)
        {
            if (CurrentMood != mood)
            {
                CurrentMood = mood;
                LastMoodChange = DateTime.Now;
            }
        }

        public static void ResetMood()
        {
            SetMood(VitoMood.Normal);
            FailureStreak = 0;
            BuildTimestamps.Clear();
        }
    }
}
