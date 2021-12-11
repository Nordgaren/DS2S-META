using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DS2_META
{
    class DS2Class
    {
        private static Regex classEntryRx = new Regex(@"^(?<id>\S+) (?<sl>\S+) (?<vig>\S+) (?<end>\S+) (?<vit>\S+) (?<att>\S+) (?<str>\S+) (?<dex>\S+) (?<adp>\S+) (?<int>\S+) (?<fth>\S+) (?<name>.+)$");

        public string Name;
        public byte ID;
        public short SoulLevel;
        public short Vigor;
        public short Endurance;
        public short Vitality;
        public short Attunement;
        public short Strength;
        public short Dexterity;
        public short Adaptability;
        public short Intelligence;
        public short Faith;

        private DS2Class(string config)
        {
            Match classEntry = classEntryRx.Match(config);
            Name = classEntry.Groups["name"].Value;
            ID = Convert.ToByte(classEntry.Groups["id"].Value);
            SoulLevel = Convert.ToInt16(classEntry.Groups["sl"].Value);
            Vigor = Convert.ToInt16(classEntry.Groups["vig"].Value);
            Endurance = Convert.ToInt16(classEntry.Groups["end"].Value);
            Vitality = Convert.ToInt16(classEntry.Groups["vit"].Value);
            Attunement = Convert.ToInt16(classEntry.Groups["att"].Value);
            Strength = Convert.ToInt16(classEntry.Groups["str"].Value);
            Dexterity = Convert.ToInt16(classEntry.Groups["dex"].Value);
            Adaptability = Convert.ToInt16(classEntry.Groups["adp"].Value);
            Intelligence = Convert.ToInt16(classEntry.Groups["int"].Value);
            Faith = Convert.ToInt16(classEntry.Groups["fth"].Value);
        }

        public override string ToString()
        {
            return Name;
        }

        public static List<DS2Class> All = new List<DS2Class>();

        static DS2Class()
        {
            foreach (string line in Regex.Split(Properties.Resources.Classes, "[\r\n]+"))
            {
                if (GetTxtResourceClass.IsValidTxtResource(line)) //determine if line is a valid resource or not
                    All.Add(new DS2Class(line));
            }
        }
    }

    class DS2Level
    {
        private static Regex levelEntryRx = new Regex(@"(?<sl>\S+) (?<cost>\S+)$");
        public int Level { get; set; }
        public int Cost { get; set; }

        public DS2Level(string config)
        {
            Match classEntry = levelEntryRx.Match(config);
            Level = Convert.ToInt32(classEntry.Groups["sl"].Value);
            Cost = Convert.ToInt32(classEntry.Groups["cost"].Value);
        }

        public DS2Level(int level, int cost)
        {
            Level = level;
            Cost = cost;
        }
        public static List<DS2Level> Levels = new List<DS2Level>();
        public static List<DS2Level> LevelsPreBuilt = new List<DS2Level>();
        static DS2Level()
        {
            foreach (string line in Regex.Split(Properties.Resources.Levels, "[\r\n]+"))
            {
                if (GetTxtResourceClass.IsValidTxtResource(line)) //determine if line is a valid resource or not
                    LevelsPreBuilt.Add(new DS2Level(line));
            }
        }
    }
}
