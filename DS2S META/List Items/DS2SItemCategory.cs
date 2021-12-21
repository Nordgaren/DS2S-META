using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META
{
    internal class DS2SItemCategory
    {
        public string Name;
        public List<DS2SItem> Items;

        private DS2SItemCategory(string name, int type, string itemList, bool showIDs)
        {
            Name = name;
            Items = new List<DS2SItem>();
            foreach (string line in GetTxtResourceClass.RegexSplit(itemList, "[\r\n]+"))
            {
                if (GetTxtResourceClass.IsValidTxtResource(line)) //determine if line is a valid resource or not
                    Items.Add(new DS2SItem(line, type, showIDs));
            };
            Items.Sort();
        }
        public override string ToString()
        {
            return Name;
        }
        static DS2SItemCategory()
        {
            foreach (string line in GetTxtResourceClass.RegexSplit(GetTxtResourceClass.GetTxtResource("Resources/Equipment/DS2SItemCategories.txt"), "[\r\n]+"))
            {
                if (GetTxtResourceClass.IsValidTxtResource(line)) //determine if line is a valid resource or not
                {
                    var att = GetTxtResourceClass.RegexSplit(line, ",");
                    Array.ForEach<string>(att, x => att[Array.IndexOf<string>(att, x)] = x.Trim());
                    Debug.WriteLine(line);
                    All.Add(new DS2SItemCategory(att[0], Convert.ToInt32(att[1], 16), GetTxtResourceClass.GetTxtResource(att[2]), bool.Parse(att[3])));
                }
            };
        }

        public static List<DS2SItemCategory> All = new List<DS2SItemCategory>();
    }
}
