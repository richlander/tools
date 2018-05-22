using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace releases
{
    class MarkdownPrinter : IPrint
    {
        public void PrintHeader(List<string> headers)
        {
            var line1 = new StringBuilder();
            var line2 = new StringBuilder();

            foreach (var header in headers)
            {
                line1.Append($"| {header}");
                line2.Append("|:--:");
            }

            line1.Append('|');
            line2.Append('|');

            WriteLine(line1.ToString());
            WriteLine(line2.ToString());
        }

        public void PrintTable(List<string> keys, Dictionary<string, Dictionary<string, string>> releases)
        {
            foreach (var key in keys)
            {
                var row = releases[key];
                var line = new StringBuilder();
                line.Append($"| {row["date"]}");
                var runtime = row.TryGetValue("version-runtime", out var runtimeValue);
                var sdk = row.TryGetValue("version-sdk", out var sdkValue);
                var relNotes = row.TryGetValue("release-notes", out var relNotesValue);

                line.Append('|');

                if (runtime && sdk )
                {
                    line.Append( $".NET Core {runtimeValue} with SDK {sdkValue}");
                }
                else if (sdk)
                {
                    line.Append($".NET Core SDK {sdkValue}");
                }
                else
                {
                    line.Append($".NET Core {runtimeValue}");
                }

                line.Append($"| ");

                if (relNotes)
                {
                    line.Append(relNotesValue);
                }

                line.Append('|');
                WriteLine(line.ToString());
            }
        }
    }
}
