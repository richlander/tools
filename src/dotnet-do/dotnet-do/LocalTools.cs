using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;

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
            var dir = _dir;
            var exePath = string.Empty;
            var exeName = args[0];
            do
            {
                var toolPath = Path.Combine(dir.FullName, TOOLS_DIRECTORY_NAME);
                var toolsInstallExist = Directory.Exists(toolPath);
                if (toolsInstallExist)
                {
                    exePath = Path.Combine(toolPath, exeName);
                    break;
                }

                dir = dir.Parent;

            } while (dir != null);

            string argsString = string.Empty;
            if (args.Length >1)
            {
                var argSpan = new Span<string>(args, 1, args.Length);
                argsString = String.Join(' ', argSpan.ToArray());
            }

            if (exePath != string.Empty)
            {
                Process.Start(exePath,argsString);
            }
            else
            {
                Console.WriteLine("Tool not found.");
            }

        }

        public void Install()
        {
            var dir = _dir;

            tools = new Dictionary<string, Tool>();

            do
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

                dir = dir.Parent;

            } while (dir != null);

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
                Process.Start("dotnet", toolInstallParams);
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

    }
}
