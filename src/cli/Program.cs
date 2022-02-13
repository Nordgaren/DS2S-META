using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkSoulsAutoSplitter.UI;
using DarkSoulsMemory.DarkSouls2;

namespace cli
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        
        [STAThread]
        static void Main(string[] args)
        {
            //TestUi();
            //return;

            var ds2 = new DarkSouls2();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(ds2.LastBonfireAreaId + " " + ds2.LastBonfireId);
                Thread.Sleep(500);
            }

            return;
        }

        public static void TestUi()
        {
            Form f = new Form();
            var c = new MainControlFormsWrapper();
            f.Width = c.Width;
            f.Height = c.Height;
            f.Controls.Add(c);
            f.ShowDialog();
        }
    }
}
