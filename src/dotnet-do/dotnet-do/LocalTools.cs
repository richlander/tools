using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dotnet_do
{
    public class LocalTools
    {
        private DirectoryInfo _dir;
        private Dictionary<string,Tool> tools;
        private static readonly string TOOLS_LIST_FILENAME = "dotnet.tools";
        public LocalTools(DirectoryInfo location)
        {
            _dir = location;
        }

        public IEnumerable<Tool> GetTools()
        {
            tools = new Dictionary<string, Tool>();

            DirectoryInfo dir = _dir;
            while (dir.Parent != null)
            {
                dir = dir.Parent;
                var toolsListExist = File.Exists(TOOLS_LIST_FILENAME);
                if (toolsListExist)
                {
                    var toolPath = Path.Combine(dir.FullName, TOOLS_LIST_FILENAME);
                    var dirTools = GetToolsFomPath(toolPath);

                    foreach(var tool in dirTools)
                    {
                        if (!tools.ContainsKey(tool.Name))
                        {
                            tools.Add(tool.Name, tool);
                        }
                    }
                }
            }

            foreach (var tool in tools)
            {
                yield return tool.Value;
            }
        }

        public List<Tool> GetToolsFomPath(string toolsPath)
        {
            var tools = new List<Tool>();

            using (var reader = new StreamReader(toolsPath))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var tool = new Tool();
                    var array = line.Split(',');
                    if (array.Length>2)
                    {
                        throw new Exception();
                    }
                    else if (array.Length == 2)
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
