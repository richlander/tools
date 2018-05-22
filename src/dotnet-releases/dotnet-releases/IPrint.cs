using System;
using System.Collections.Generic;
using System.Text;

namespace releases
{
    public interface IPrint
    {
        void PrintHeader();
        void PrintTable(List<string> keys, Dictionary<string, Dictionary<string, string>> releases);
    }
}
