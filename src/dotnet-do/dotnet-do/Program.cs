using System;
using System.Diagnostics;
using System.IO;

namespace dotnet_do
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var currentDir = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(currentDir);
            var tools = new LocalTools(dir);

            if (args == null || args.Length == 0)
            {
                tools.Install();
            }
            else
            {
                tools.Run(args);
            }
        }

        private static void Run()
        {
            
        }


    }
}
