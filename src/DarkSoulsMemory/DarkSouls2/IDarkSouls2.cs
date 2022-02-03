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

    }
}
