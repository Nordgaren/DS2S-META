using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace cli
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        
        [STAThread]
        static void Main(string[] args)
        {
            AutoSplitterRunner.Run();
            return;



            var process = Process.GetProcessesByName("DarkSoulsII").FirstOrDefault();
            var start = 0x7FF45234A0E0;
            var end = 0x7FF45234C572;
            var extend = 40000;

            int size = (int)( (end + extend) - (start - extend));
            size -= (size % 8);//crudely allign to 8 bytes
            var startAddress = (IntPtr)(start - extend);
            var bytes = ReadBytes(process.Handle, (IntPtr)(start - extend), size);

            var sb = new StringBuilder();
            for(int i = 0; i * 8 < size; i+=8)
            {
                sb.Append($"0x{(start - extend) + i * 8 :X} ");
                for(int j = 0; j < 8; j++)
                {
                    var index = (i * 8) + j;
                    sb.Append($"{bytes[(i * 8) + j]:x2} ");
                }
                sb.Append($"\n");
            }
            var result = sb.ToString();
        }

        static byte[] ReadBytes(IntPtr handle, IntPtr address, int count)
        {
            int bytesRead = 0;
            byte[] bytes = new byte[count];

            ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

            return bytes;
        }
    }
}
