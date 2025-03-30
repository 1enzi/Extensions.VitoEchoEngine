using Extensions.VitoEchoEngine.Models.Enum;
using Extensions.VitoEchoEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Extensions.VitoEchoEngine.Utils
{
    public static class EmbeddedQuoteLoader
    {
        public static Dictionary<string, Dictionary<VitoMood, List<string>>> LoadMainQuotes()
        {
            var json = LoadEmbeddedJson("MainQuotes.json");

            return JsonConvert
                .DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(json)
                .ToDictionary(
                    outer => outer.Key,
                    outer => outer.Value.ToDictionary(
                        inner =>
                        {
                            Enum.TryParse<VitoMood>(inner.Key, out var mood);
                            return mood;
                        },
                        inner => inner.Value
                    )
                );
        }

        public static DevOverlayQuotes LoadDevOverlayQuotes()
        {
            var json = LoadEmbeddedJson("DevOverlayQuotes.json");
            return JsonConvert.DeserializeObject<DevOverlayQuotes>(json);
        }

        private static string LoadEmbeddedJson(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(fileName)) ?? throw new FileNotFoundException($"Embedded resource '{fileName}' not found.");

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
