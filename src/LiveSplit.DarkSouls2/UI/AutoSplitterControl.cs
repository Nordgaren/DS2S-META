using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveSplit.DarkSouls2.Splits;

namespace LiveSplit.DarkSouls2
{
    public partial class AutoSplitterControl : UserControl
    {
        public BindingList<ISplit> Splits = new BindingList<ISplit>();


        public AutoSplitterControl()
        {
            InitializeComponent();
        }
    }
}
