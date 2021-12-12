using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DS2S_META
{
    class DS2SBonfire : IComparable<DS2SBonfire>
    {
        private static Regex bonfireEntryRx = new Regex(@"^(?<id>\S+) (?<name>.+)$");

        public string Name;
        public int ID;

        private DS2SBonfire(string config)
        {
            Match bonfireEntry = bonfireEntryRx.Match(config);
            Name = bonfireEntry.Groups["name"].Value;
            ID = Convert.ToInt32(bonfireEntry.Groups["id"].Value);
        }

        public DS2SBonfire(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(DS2SBonfire other)
        {
            return Name.CompareTo(other.Name);
        }

        public static List<DS2SBonfire> All = new List<DS2SBonfire>();

        static DS2SBonfire()
        {
            foreach (string line in Regex.Split(GetTxtResourceClass.GetTxtResource("Resources/Systems/Bonfires.txt"), "[\r\n]+"))
            {
                if (GetTxtResourceClass.IsValidTxtResource(line)) //determine if line is a valid resource or not
                    All.Add(new DS2SBonfire(line));
            };
            All.Sort();
        }
    }
}
