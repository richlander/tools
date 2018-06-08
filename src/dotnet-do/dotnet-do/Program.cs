using System;
using System.Diagnostics;
using System.IO;
using static System.Console;

namespace dotnet_do
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(currentDir);
            var tools = new LocalTools(dir);

            if (args == null || args.Length == 0)
            {
                WriteLine("This tool exposes two options:");
                WriteLine("install -- installs tools referenced in dotnet.tool files.");
                WriteLine("clean -- deletes installed tools in .dotnettools directories");
                WriteLine("print -- prints set of dotnet.tools files.");
                WriteLine("[toolname] -- runs a tool (using short name w/no extension).");
            }
            else if (args[0] == "install")
            {
                tools.Install();
            }
            else if (args[0] == "clean")
            {
                tools.Clean();
            }
            else if (args[0] == "print")
            {
                tools.Print();
            }

            else
            {
                tools.Run(args);
            }
        }
    }
}
