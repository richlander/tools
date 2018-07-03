using System;
using System.IO;
using static System.Console;

namespace cog
{
    class Program
    {

        private static StreamWriter _writer;
        static void Main(string[] args)
        {
            var reader = In;
            if (!IsInputRedirected)
            {
                WriteLine("No piped input to log.");
                return;
            }
            else if (args == null || args.Length !=1)
            {
                WriteLine("No filename specified.");
                WriteLine("Press CTRL-C to exit");
                return;
            }

            CancelKeyPress += (object sender, ConsoleCancelEventArgs cargs) =>
            {
                WriteToFile();
                CloseFile();
                var p =System.Diagnostics.Process.GetCurrentProcess();
                p.Kill();
            };
            var filename = args[0];
            _writer = new StreamWriter(filename);
            WriteToFile();
            CloseFile();
        }

        static void WriteToFile()
        {
            var line = string.Empty;
            while ((line = In.ReadLine()) != null)
            {
                _writer.WriteLine(line);
                WriteLine(line);
            }

        }

        static void CloseFile()
        {
            _writer.Close();
        }
    }
}
