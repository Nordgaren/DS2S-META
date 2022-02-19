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
using DarkSoulsMemory.DarkSouls1;
using DarkSoulsMemory.DarkSouls2;
using DarkSoulsMemory.Memory;
using DarkSoulsMemory.Shared;
using WarpType = DarkSoulsMemory.DarkSouls1.WarpType;

namespace cli
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var process = Process.GetProcesses().FirstOrDefault(i => i.ProcessName.ToLower().StartsWith("dark"));



            var ds1 = new DarkSouls1();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(ds1.GetBonfireState(Bonfire.AnorLondoPrincess));
                Thread.Sleep(1000);
            }


           

            
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

        private static void Testy()
        {
            Pointer bossKillCount;
            Pointer AiManager;
            Pointer rightHandWeaponMultiplier;
            Pointer LeftHandWeaponMultiplier;

            var process = Process.GetProcesses().FirstOrDefault(i => i.ProcessName.StartsWith("DarkSoulsII"));

            process.ScanPatternRelative("48 8B 05 ?? ?? ?? ?? 48 8B 58 38 48 85 DB 74 ?? F6", 3, 7)
                .CreatePointer(out bossKillCount, 0x70, 0x28, 0x20, 0x8)
                .CreatePointer(out AiManager, 0x28)
                .CreatePointer(out rightHandWeaponMultiplier, 0xd0, 0x378, 0x28, 0x158)
                .CreatePointer(out LeftHandWeaponMultiplier, 0xd0, 0x378, 0x28, 0x80)
                ;

            //Disable AI
            AiManager.WriteBool(0x18, true);

            //Set crazy damage
            rightHandWeaponMultiplier.WriteFloat(999999.0f);

            //Read boss kill counters
            var lastGiantKillCount = bossKillCount.ReadInt32(0x7c);

            while (true)
            {
                Console.Clear();
                var res = bossKillCount.ToString();
                Console.WriteLine(res);
            }
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
