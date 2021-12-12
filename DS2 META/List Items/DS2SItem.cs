using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META
{
    class DS2SItem : IComparable<DS2SItem>
    {
        public enum Upgrade
        {
            None = 0,
            Unique = 1,
            Armor = 2,
            Infusable = 3,
            InfusableRestricted = 4,
            PyroFlame = 5,
            PyroFlameAscended = 6,
        }

        private static Regex itemEntryRx = new Regex(@"^\s*(?<id>\S+)\s+(?<limit>\S+)\s+(?<upgrade>\S+)\s+(?<name>.+)$");

        private bool mystery;

        public string Name;
        public int ID;
        public int StackLimit;
        public Upgrade UpgradeType;
        public int CategoryID;

        public DS2SItem(string config, bool showID, int categoryID)
        {
            CategoryID = categoryID;
            Match itemEntry = itemEntryRx.Match(config);
            ID = Convert.ToInt32(itemEntry.Groups["id"].Value);
            StackLimit = Convert.ToInt32(itemEntry.Groups["limit"].Value);
            UpgradeType = (Upgrade)Convert.ToInt32(itemEntry.Groups["upgrade"].Value);
            mystery = showID;
            if (showID)
                Name = ID.ToString() + ": " + itemEntry.Groups["name"].Value;
            else
                Name = itemEntry.Groups["name"].Value;
        }

        public static List<int> NO = new List<int>()
        {
            5050000,5051000,5052000,5053000,5061000,5062000,5071000,5072000,5081000,5082000,5091000,5092000,5101000,5102000,6900000,6901000,6902000,
            6903000,6910000,6911000,6912000,6913000,6920000,6921000,6922000,6923000,6930000,6931000,6932000,6933000,6940000,6941000,6942000,6943000,
            6950000,6951000,6952000,6953000,6960000,6961000,6962000,6963000,6990000,6991000,6992000,6993000,1700000,1701000,1702000,1703000,1704000,
            1705000,1706000,1707000,1708000,1709000,1710000,1711000,1712000,1713000,1714000,1715000
        };

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(DS2SItem other)
        {
            if (mystery)
                return ID.CompareTo(other.ID);
            else
                return Name.CompareTo(other.Name);
        }
    }
}
