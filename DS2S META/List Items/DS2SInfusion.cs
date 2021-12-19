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
        public int ID;
        private DS2SInfusion(string name, int value)
        {
            Name = name;
            ID = value;

        }

        public override string ToString()
        {
            return Name;
        }

        public static DS2SInfusion Normal = new DS2SInfusion("Normal", 0);
        public static DS2SInfusion Bleed = new DS2SInfusion("Bleed", 6);
        public static DS2SInfusion Dark = new DS2SInfusion("Dark", 4);
        public static DS2SInfusion Enchanted = new DS2SInfusion("Enchanted", 8);
        public static DS2SInfusion Fire = new DS2SInfusion("Fire", 1);
        public static DS2SInfusion Lightning = new DS2SInfusion("Lightning", 3);
        public static DS2SInfusion Magic = new DS2SInfusion("Magic", 2);
        public static DS2SInfusion Mundane = new DS2SInfusion("Mundane", 9);
        public static DS2SInfusion Poison = new DS2SInfusion("Poison", 5);
        public static DS2SInfusion Raw = new DS2SInfusion("Raw", 7);

        public static List<DS2SInfusion> Infusions = new List<DS2SInfusion>()
        {
                    new DS2SInfusion("Normal", 0),
                    new DS2SInfusion("Fire", 1),
                    new DS2SInfusion("Magic", 2),
                    new DS2SInfusion("Lightning", 3),
                    new DS2SInfusion("Dark", 4),
                    new DS2SInfusion("Poison", 5),
                    new DS2SInfusion("Bleed", 6),
                    new DS2SInfusion("Raw", 7),
                    new DS2SInfusion("Enchanted", 8),
                    new DS2SInfusion("Mundane", 9)
        };
    }
}
