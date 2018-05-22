using System;
using System.Threading.Tasks;

namespace releases
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var releases = await Releases.GetReleasesAsync();

            var markdownprinter = new MarkdownPrinter();
            releases.Print(markdownprinter);
            
        }
    }
}
