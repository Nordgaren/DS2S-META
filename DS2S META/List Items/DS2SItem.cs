using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DS2S_META
{
    class DS2SItem : IComparable<DS2SItem>
    {
       
        private static Regex itemEntryRx = new Regex(@"^\s*(?<id>\S+)\s+(?<limit>\S+)\s+(?<infusion>\S+)\s+(?<max>\S+)\s+(?<name>.+)$");

        private bool ShowID;

        public string Name;
        public int ID;
        public int StackLimit;
        public DS2SInfusion.InfusionType Infusion;
        public int MaxUpgrade;

        public DS2SItem(string config, bool showID)
        {
            Match itemEntry = itemEntryRx.Match(config);
            ID = Convert.ToInt32(itemEntry.Groups["id"].Value);
            StackLimit = Convert.ToInt32(itemEntry.Groups["limit"].Value);
            Infusion = (DS2SInfusion.InfusionType)Convert.ToInt32(itemEntry.Groups["infusion"].Value);
            MaxUpgrade = Convert.ToInt32(itemEntry.Groups["max"].Value);
            ShowID = showID;
            if (showID)
                Name = ID.ToString() + ": " + itemEntry.Groups["name"].Value;
            else
                Name = itemEntry.Groups["name"].Value;
        }
        public override string ToString()
        {
            return Name;
        }
        public int CompareTo(DS2SItem other)
        {
            if (ShowID)
                return ID.CompareTo(other.ID);
            else
                return Name.CompareTo(other.Name);
        }
    }
}
