using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static System.Console;

namespace dotnet_do
{
    public class LocalTools
    {
        private DirectoryInfo _dir;
        private Dictionary<string,Tool> tools;
        private static readonly string TOOLS_LIST_FILENAME = "dotnet.tools";
        private static readonly string TOOLS_DIRECTORY_NAME = ".dotnettools";

        public LocalTools(DirectoryInfo location)
        {
            _dir = location;
        }

        public string ToolPath
        {
            get; private set;
        }

        public void Run(string[] args)
        {
            var exePath = string.Empty;
            var exeName = args[0];
            foreach (var dir in EnumerateDirectoriesUp(_dir))
            {
                var toolPath = Path.Combine(dir.FullName, TOOLS_DIRECTORY_NAME);
                var toolsInstallExist = Directory.Exists(toolPath);
                if (toolsInstallExist)
                {
                    exePath = Path.Combine(toolPath, exeName);
                    break;
                }
            }

            string argsString = string.Empty;
            if (args.Length >1)
            {
                var argSpan = new Span<string>(args, 1, args.Length-1);
                argsString = String.Join(' ', argSpan.ToArray());
            }

            if (exePath != string.Empty)
            {
                var startinfo = new ProcessStartInfo();
                startinfo.RedirectStandardOutput = true;
                startinfo.FileName = exePath;
                startinfo.Arguments = argsString;
                var process = Process.Start(startinfo);
                var output = process.StandardOutput.ReadToEnd();
                WriteLine(output);
            }
            else
            {
                Console.WriteLine("Tool not found.");
            }

        }

        public void Install()
        {
            tools = new Dictionary<string, Tool>();

            foreach(var dir in EnumerateDirectoriesUp(_dir))
            {
                var toolPath = Path.Combine(dir.FullName, TOOLS_LIST_FILENAME);
                var toolsListExist = File.Exists(toolPath);
                if (toolsListExist)
                {
                    if (ToolPath == null)
                    {
                        ToolPath = Directory.GetParent(toolPath).FullName;
                    }

                    var dirTools = GetToolsFomPath(toolPath);

                    foreach (var tool in dirTools)
                    {
                        if (!tools.ContainsKey(tool.Name))
                        {
                            tools.Add(tool.Name, tool);
                        }
                    }
                }
            }

            if (tools.Keys.Count == 0)
            {
                Console.WriteLine($"{TOOLS_DIRECTORY_NAME} was not found.");
                return;
            }

            foreach (var t in tools)
            {
                var tool = t.Value;
                var toolString = string.IsNullOrWhiteSpace(tool.Version) ? $"{tool.Name}" : $"{tool.Name} --version {tool.Version}";
                var toolPath = Path.Combine(ToolPath, TOOLS_DIRECTORY_NAME);
                var toolInstallParams = $"tool install --tool-path {toolPath} {toolString}";
                var process = Process.Start("dotnet", toolInstallParams);
                process.WaitForExit();
            }
        }

        public void Clean()
        {
            foreach (var dir in EnumerateDirectoriesUp(_dir))
            {
                var toolPath = Path.Combine(dir.FullName, TOOLS_DIRECTORY_NAME);
                var toolsInstallExist = Directory.Exists(toolPath);
                if (toolsInstallExist)
                {
                    WriteLine($"Deleting: {toolPath}");
                    Directory.Delete(toolPath,true);
                }
            }
        }

        public void Print()
        {
            foreach (var dir in EnumerateDirectoriesUp(_dir))
            {
                var toolPath = Path.Combine(dir.FullName, TOOLS_LIST_FILENAME);
                var toolsListExist = File.Exists(toolPath);
                if (toolsListExist)
                {
                    WriteLine(toolPath);
                }
            }

        }

        private List<Tool> GetToolsFomPath(string toolsPath)
        {
            var tools = new List<Tool>();

            using (var reader = new StreamReader(toolsPath))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    else if (line[0] == '#')
                    {
                        continue;
                    }

                    var tool = new Tool();
                    var array = line.Split(',');

                    if (array.Length > 1)
                    {
                        tool.Name = array[0];
                        tool.Version = array[1];
                    }
                    else
                    {
                        tool.Name = array[0];
                    }

                    tools.Add(tool);
                }
                return tools;
            }
        }

        private IEnumerable<DirectoryInfo> EnumerateDirectoriesUp(DirectoryInfo directory)
        {
            var dir = directory;
            do
            {
                yield return dir;
                dir = dir.Parent;
            } while (dir != null);
        }
    }
}
