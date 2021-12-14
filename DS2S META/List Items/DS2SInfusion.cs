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

        public enum InfusionType
        {
            None = 0,
            Melee = 1,
            Ranged = 2,
            Shield = 3,
            Chimes = 4,
            Staves = 5
        }

        private DS2SInfusion(string name, int value, params InfusionType[] infusions)
        {
            Name = name;
            ID = value;

            foreach (var infusion in infusions)
            {
                InfusionDict[infusion].Add(this);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public static Dictionary<InfusionType, List<DS2SInfusion>> InfusionDict = new Dictionary<InfusionType, List<DS2SInfusion>>()
        {
            {InfusionType.None, new List<DS2SInfusion>() {} },
            {InfusionType.Melee, new List<DS2SInfusion>() {} },
            {InfusionType.Ranged, new List<DS2SInfusion>() {} },
            {InfusionType.Shield, new List<DS2SInfusion>() {} },
            {InfusionType.Chimes, new List<DS2SInfusion>() {} },
            {InfusionType.Staves, new List<DS2SInfusion>() {} }
        };

        public static void BuildInfusionDicts()
        {
            new DS2SInfusion("Normal", 0, InfusionType.None, InfusionType.Melee, InfusionType.Ranged, InfusionType.Chimes, InfusionType.Staves, InfusionType.Shield);
            new DS2SInfusion("Bleed", 6, InfusionType.Melee, InfusionType.Shield);
            new DS2SInfusion("Dark", 4, InfusionType.Melee, InfusionType.Ranged, InfusionType.Chimes, InfusionType.Staves, InfusionType.Shield);
            new DS2SInfusion("Enchanted", 8, InfusionType.Melee, InfusionType.Ranged);
            new DS2SInfusion("Fire", 1,  InfusionType.Melee, InfusionType.Ranged, InfusionType.Shield);
            new DS2SInfusion("Lightning", 3, InfusionType.Melee, InfusionType.Ranged, InfusionType.Chimes, InfusionType.Shield);
            new DS2SInfusion("Magic", 2, InfusionType.Melee, InfusionType.Ranged, InfusionType.Staves, InfusionType.Shield);
            new DS2SInfusion("Mundane", 9, InfusionType.Melee, InfusionType.Ranged);
            new DS2SInfusion("Poison", 5, InfusionType.Melee, InfusionType.Shield);
            new DS2SInfusion("Raw", 7, InfusionType.Melee, InfusionType.Ranged);
        }
    }
}
