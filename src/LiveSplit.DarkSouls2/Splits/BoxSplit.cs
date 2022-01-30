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
        

        public float LowerX
        {
            get => _lowerX;
            set
            {
                _lowerX = value;
                OnPropertyChanged();
            }
        }
        private float _lowerX;
        public float LowerY
        {
            get => _lowerY;
            set
            {
                _lowerY = value;
                OnPropertyChanged();
            }
        }
        private float _lowerY;
        public float LowerZ
        {
            get => _lowerZ;
            set
            {
                _lowerZ = value;
                OnPropertyChanged();
            }
        }
        private float _lowerZ;

        public float UpperX
        {
            get => _upperX;
            set
            {
                _upperX = value;
                OnPropertyChanged();
            }
        }
        private float _upperX;
        public float UpperY
        {
            get => _upperY;
            set
            {
                _upperY = value;
                OnPropertyChanged();
            }
        }
        private float _upperY;
        public float UpperZ
        {
            get => _upperZ;
            set
            {
                _upperZ = value;
                OnPropertyChanged();
            }
        }
        private float _upperZ;






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
