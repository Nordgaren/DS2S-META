using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using DarkSoulsMemory.DarkSouls2.Sotfs;
using NUnit.Framework;
using Testing.Tas;

namespace Testing
{
    [TestFixture]
    internal class DarkSouls2
    {
        private static DarkSouls2ToolAssistant _assistant;
        private static DarkSouls2SotfsHook _darkSouls2; 
        private static string SaveFileLocation = @"C:\Users\Frank\AppData\Roaming\DarkSoulsII\0110000104593e46";
        private static string SaveFileName = @"DS2SOFS0000.sl2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var ReplacementSave = "Sotfs";

            //Rename current save
            if (File.Exists(SaveFileLocation + "\\" + SaveFileName))
            {
                if (File.Exists(SaveFileLocation + "\\automated_backup"))
                {
                    File.Delete(SaveFileLocation + "\\automated_backup");
                }
                File.Move(SaveFileLocation + "\\" + SaveFileName, SaveFileLocation + "\\automated_backup");
            }
            //Replace with test save
            File.Move(Environment.CurrentDirectory + $@"\saves\DarkSouls2\{ReplacementSave}\ThingsBetwixt", SaveFileLocation + "\\" + SaveFileName);




            _assistant = new DarkSouls2ToolAssistant(GameType.DarkSouls2Sotfs);
            _darkSouls2 = new DarkSouls2SotfsHook(10, 5000);
            _darkSouls2.Start();

            _darkSouls2.RightHand1DamageMultiplier = 9999;

            _assistant.MainMenuContinue();
        }

        
        [Test]
        public void BossKill()
        {
            KillLastGiant();
        }


        #region Boss kills

        private static void KillLastGiant()
        {
            _darkSouls2.Refresh();
            Warp("Cardinal Tower");

            Teleport(-144.2992f, 93.41512f, -40.57851f, 0.99f, 0.17f, 0.0f);
            EnterFogGate();
            _assistant.SkipCutscene();
        }

        #endregion



        #region Helpers

        private static void EnterFogGate()
        {
            _assistant.Interact();
            Thread.Sleep(4000);
        }

        private static void Teleport(float x, float y, float z, float angX = 0.0f, float angY = 0.0f, float angZ = 0.0f)
        {
            _darkSouls2.StableX = x;
            _darkSouls2.StableY = y;
            _darkSouls2.StableZ = z;
            _darkSouls2.AngX = angX;
            _darkSouls2.AngY = angY;
            _darkSouls2.AngZ = angZ;
        }

        private static void Warp(string bonfireName)
        {
            var bonfire = _darkSouls2.Bonfires.First(i => i.Name == bonfireName);
            _darkSouls2.LastBonfireID = bonfire.BonfireId;
            _darkSouls2.LastBonfireAreaID = bonfire.AreaId;
            if (_darkSouls2.Warp(bonfire.BonfireId))
            {
                Thread.Sleep(7000);
            }
        }

        #endregion

    }
}
