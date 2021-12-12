using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META
{
    class DS2SInfusion
    {
        public string Name;
        public int Value;
        public int MaxUpgrade;
        public bool Restricted;

        private DS2SInfusion(string name, int value, int maxUpgrade, bool restricted)
        {
            Name = name;
            Value = value;
            MaxUpgrade = maxUpgrade;
            Restricted = restricted;
        }

        public override string ToString()
        {
            return Name;
        }

        public static List<DS2SInfusion> All = new List<DS2SInfusion>()
        {
            new DS2SInfusion("Normal", 0, 15, false),
            new DS2SInfusion("Bleed", 6, 5, true),
            new DS2SInfusion("Dark", 4, 5, true),
            new DS2SInfusion("Enchanted", 8, 5, true),
            new DS2SInfusion("Fire", 1, 10, false),
            new DS2SInfusion("Lightning", 3, 5, false),
            new DS2SInfusion("Magic", 2, 10, false),
            new DS2SInfusion("Mundane", 9, 10, false),
            new DS2SInfusion("Poison", 5, 5, false),
            new DS2SInfusion("Raw", 7, 5, true),
        };
    }
}
