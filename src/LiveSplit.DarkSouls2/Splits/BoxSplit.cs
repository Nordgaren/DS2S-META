using DarkSoulsMemory.DarkSouls1;
using DarkSoulsMemory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls2.Splits
{
    public class BoxSplit : ISplit
    { 
        public SplitType SplitType => SplitType.Box;

        private Vector3f _lowerBound;
        public Vector3f LowerBound
        {
            get => _lowerBound;
            set
            {
                _lowerBound = value;
                OnPropertyChanged();
            }
        }

        private Vector3f _upperBound;
        public Vector3f UpperBound
        {
            get => _upperBound;
            set
            {
                _upperBound = value;
                OnPropertyChanged();
            }
        }

        private TimingType _timingType;
        public TimingType TimingType
        {
            get => _timingType;
            set
            {
                _timingType = value;
                OnPropertyChanged();
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
