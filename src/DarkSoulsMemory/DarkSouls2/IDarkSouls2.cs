using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsMemory.DarkSouls2
{
    public interface IDarkSouls2
    {

        /// <summary>
        /// Returns the kill count of a given boss, affected by ng+ state and bonfire ascetics.
        /// </summary>
        /// <param name="bossType"></param>
        /// <returns></returns>
        int GetBossKillCount(BossType bossType);



        /// <summary>
        /// Setting this to true will disable the AI globally
        /// </summary>
        bool DisableAllAi { get; set; }

        /// <summary>
        /// Gets/sets the damage multiplier for right weapon 1. Default value is 1
        /// </summary>
        float RightWeapon1DamageMultiplier { get; set; }
    }
}
