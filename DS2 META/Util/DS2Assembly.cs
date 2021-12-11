using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DS2_META
{
    // Parses output from https://defuse.ca/online-x86-assembler.htm
    // I like to keep the whole thing for quick reference to line numbers and so on
    static class DS2Assembly
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
        //public static byte[] LeaveSession = loadDefuseOutput(Properties.Resources.LeaveSession);
        //public static byte[] KickPlayer = loadDefuseOutput(Properties.Resources.KickPlayer);
        //public static byte[] TargetedEntity = loadDefuseOutput(Properties.Resources.TargetedEntity);
        //public static byte[] TriggerSign = loadDefuseOutput(Properties.Resources.TriggerSign);
    }
}
