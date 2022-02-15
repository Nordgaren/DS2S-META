using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsMemory.Shared
{
    public static class PatternScanner
    {
        #region Code cache

        private static Dictionary<string, byte[]> _memoryCache = new Dictionary<string, byte[]>();
        private static byte[] GetCodeMemory(Process p)
        {
            var name = p.ProcessName;
            byte[] buffer;

            if (!_memoryCache.TryGetValue(name, out buffer))
            {
                buffer = new byte[p.MainModule.ModuleMemorySize];
                int read = 0;
                Kernel32.ReadProcessMemory(p.Handle, p.MainModule.BaseAddress, buffer, buffer.Length, ref read);
                _memoryCache[name] = buffer;
            }
            return buffer;
        }

        #endregion

        public static long RelativeScan(Process p, string pattern, int addressOffset, int instructionSize)
        {
            var mods = p.Modules;

            var scanResult = Scan(p, pattern);
            if (scanResult != 0)
            {
                var codeBytes = GetCodeMemory(p);

                var address = BitConverter.ToInt32(codeBytes, (int)scanResult + addressOffset);
                var result =  BitConverter.ToInt64(codeBytes, (int)scanResult + address + instructionSize);
                return result;
            }

            return 0;
        }


        public static long Scan(Process p, string pattern)
        {
            return Scan(p, ToPattern(pattern));
        }


        public static long Scan(Process p, byte?[] pattern)
        {
            var codeBytes = GetCodeMemory(p);
            
            //Find the pattern
            for (int i = 0; i < codeBytes.Length - pattern.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (pattern[j] != null)
                    {
                        if (pattern[j] != codeBytes[i + j])
                        {
                            found = false;
                            break;
                        }
                    }
                }

                if (found)
                {
                    return i;
                }
            }
            return 0;
        }

        
        private static byte?[] ToPattern(string pattern)
        {
            var result = new List<byte?>();
            pattern = pattern.Replace("\r", string.Empty).Replace("\n", string.Empty);
            var split = pattern.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var s in split)
            {
                if (s != "?" && s != "??" && s != "x" && s != "xx")
                {
                    result.Add(Convert.ToByte(s, 16));
                }
                else
                {
                    result.Add(null);
                }
            }
            return result.ToArray();
        }
    }
}
