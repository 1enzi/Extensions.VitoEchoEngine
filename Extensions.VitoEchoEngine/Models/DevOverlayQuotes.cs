using System.Collections.Generic;

namespace Extensions.VitoEchoEngine.Models
{
    public class DevOverlayQuotes
    {
        public Dictionary<string, List<string>> TimeBased { get; set; } = new();
        public Dictionary<string, List<string>> Other { get; set; } = new();
    }
}
