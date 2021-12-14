﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DS2S_META
{
    // Taken from DSR Gadget because TK code is better than anything I could write.
    // Parses output from https://defuse.ca/online-x86-assembler.htm
    // I like to keep the whole thing for quick reference to line numbers and so on
    static class DS2SAssembly
    {
        private static Regex asmLineRx = new Regex(@"^[\w\d]+:\s+((?:[\w\d][\w\d] ?)+)");

        private static byte[] loadDefuseOutput(string lines)
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

        public static byte[] AddSouls = loadDefuseOutput(Properties.Resources.AddSouls);
        public static byte[] GetItem = loadDefuseOutput(Properties.Resources.GiveItemWithMenu);
        public static byte[] GetItemNoMenu = loadDefuseOutput(Properties.Resources.GiveItemWithoutMenu);
    }
}
