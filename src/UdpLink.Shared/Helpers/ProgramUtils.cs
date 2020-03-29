using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace UdpLink.Shared.Helpers
{
    public class ProgramUtils
    {
        public static string GetBaseDir()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}
