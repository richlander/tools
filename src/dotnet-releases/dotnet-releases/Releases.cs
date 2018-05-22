using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace releases
{
    public class Releases
    {
        static readonly string RELEASES_JSON = "https://raw.githubusercontent.com/dotnet/core/master/release-notes/releases.json";

        public Dictionary<string, Dictionary<string, string>> Entries = null;
        public List<string> OrderedReleases = null;

        private Releases()
        {
        }

        public static Task<Releases> GetReleasesAsync()
        {
            return GetReleasesAsync(RELEASES_JSON);
        }
                
        public static async Task<Releases> GetReleasesAsync(string releasesUrl)
        {
            var entries = new Dictionary<string, Dictionary<string, string>>();
            
            var release = new Releases();
            var client = new HttpClient();
            var json = await client.GetStringAsync(releasesUrl);
            var releaseData = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(json);
            
            foreach (var entry in releaseData)
            {
                string key;
                string date = entry["date"];

                if (entry.TryGetValue("version-runtime", out var runtimeValue))
                {
                    key = $"{runtimeValue}-runtime-{date}";
                }
                else if (entry.TryGetValue("version-sdk", out var sdkValue))
                {
                    key = $"{sdkValue}-sdk-{date}";
                }
                else
                {
                    throw new Exception("Unexpected json format");
                }

                entries.Add(key, entry);
            }

            release.Entries = entries;
            var orderedKeys = entries.Keys.OrderByDescending(x => x).ToList();
            release.OrderedReleases = orderedKeys;

            return release;
        }

        public void Print(IPrint printer)
        {
            printer.PrintHeader();
            printer.PrintTable(OrderedReleases, Entries);
        }
    }
}