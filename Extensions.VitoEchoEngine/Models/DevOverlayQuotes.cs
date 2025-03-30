using System.Collections.Generic;

namespace Extensions.VitoEchoEngine.Models
{
    public class DevOverlayQuotes
    {
        public Dictionary<string, List<string>> TimeBased { get; set; } = new();
        public Dictionary<string, List<string>> MoodQuotes { get; set; } = new();
    }
}
