using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls2.Splits
{
    public interface ISplit : INotifyPropertyChanged
    {
        SplitType SplitType { get; }
    }
}
