using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsMemory.DarkSouls2
{
    public class DarkSouls2
    {
        private IDarkSouls2 _darkSouls2;
        private Process _process;

        public DarkSouls2()
        {

        }

        /// <summary>
        /// Returns true if the game is currently attached
        /// </summary>
        public bool IsAttached => _process != null;
    }
}
