using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsMemory.DarkSouls2.Sotfs;
using DarkSoulsMemory.DarkSouls2.Vanilla;

namespace DarkSoulsMemory.DarkSouls2
{
    public class DarkSouls2
    {
        private IDarkSouls2 _darkSouls2;
        private Process _process;

        public DarkSouls2()
        {
            Refresh();
        }

        /// <summary>
        /// Returns true if the game is currently attached
        /// </summary>
        public bool IsAttached => _process != null;



        /// <summary>
        /// Returns the kill count of a given boss, affected by ng+ state and bonfire ascetics.
        /// </summary>
        /// <param name="bossType"></param>
        /// <returns></returns>
        public int GetBossKillCount(BossType bossType)
        {
            return _darkSouls2?.GetBossKillCount(bossType) ?? 0;
        }


        /// <summary>
        /// Setting this to true will disable the AI globally
        /// </summary>
        public bool DisableAllAi
        {
            get => _darkSouls2 != null && _darkSouls2.DisableAllAi;
            set
            {
                if (_darkSouls2 != null)
                {
                    _darkSouls2.DisableAllAi = value;
                }
            }
        }

        /// <summary>
        /// Gets/sets the damage multiplier for right weapon 1. Default value is 1
        /// </summary>
        public float RightWeapon1DamageMultiplier
        {
            get
            {
                if (_darkSouls2 != null)
                {
                    return _darkSouls2.RightWeapon1DamageMultiplier;
                }
                return 0;
            }
            set
            {
                if (_darkSouls2 != null)
                {
                    _darkSouls2.RightWeapon1DamageMultiplier = value;
                }
            }
        }

        /// <summary>
        /// Refreshes the process attachment, should be called every frame. Returns true if the game is attached
        /// </summary>
        /// <returns></returns>
        public bool Refresh()
        {
            if (_darkSouls2 == null)
            {
                var process = Process.GetProcesses().FirstOrDefault(i => i.ProcessName.StartsWith("DarkSoulsII"));
                if (process != null)
                {
                    var isScholar = process.MainModule.FileName.ToLower().Contains("scholar");

                    if (isScholar)
                    {
                        var dsHook = new DarkSouls2SotfsHook(5000, 5000);
                        dsHook.Refresh();
                        _darkSouls2 = dsHook;
                    }
                    else
                    {
                        var dsHook = new DarkSouls2VanillaHook(5000, 5000);
                        dsHook.Refresh();
                        _darkSouls2 = dsHook;
                    }
                }
            }
            else
            {
                //if (!_darkSouls2.Setup())
                //{
                //    _darkSouls2 = null;
                //}
            }

            return _darkSouls2 != null;
        }
    }
}
