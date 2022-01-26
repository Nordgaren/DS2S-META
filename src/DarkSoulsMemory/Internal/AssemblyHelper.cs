using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarkSoulsMemory.Internal
{
    //Stolen from Nordgaren, https://github.com/Nordgaren/DSR-Gadget-Local-Loader/blob/master/DSR-Gadget/Util/DSRAssembly.cs
    internal static class AssemblyHelper
    {
        private static Regex asmLineRx = new Regex(@"^[\w\d]+:\s+((?:[\w\d][\w\d] ?)+)");

        public static byte[] LoadDefuseOutput(string lines)
        {
            List<byte> bytes = new List<byte>();
            foreach (string line in Regex.Split(lines, "[\r\n]+"))
            {
                Match match = asmLineRx.Match(line);
                string hexes = match.Groups[1].Value;
                foreach (Match hex in Regex.Matches(hexes, @"\S+"))
                    bytes.Add(Byte.Parse(hex.Value, System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            return bytes.ToArray();
        }


        public static byte[] FasmAssemble(string asm)
        {
            var path = Environment.CurrentDirectory + "\\Fasm32.exe";

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = path,
                    Arguments = "\"" + asm + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                },
            };

            process.Start();
            process.WaitForExit();

            while (!process.StandardOutput.EndOfStream)
            {
                var asd = process.StandardOutput.ReadLine();
                // do something with line
            }

            string err = process.StandardError.ReadToEnd();

            string line = process.StandardOutput.ReadToEnd();
            return null;
        }
    }
}
