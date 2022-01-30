using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls2.Splits;

namespace LiveSplit.DarkSouls2.UI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SplitViewModel> Splits { get; set; } = new ObservableCollection<SplitViewModel>();


        



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SplitViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SplitType> SplitTypesItemsSource { get; set; } = new ObservableCollection<SplitType>(Enum.GetValues(typeof(SplitType)).Cast<SplitType>());


        public SplitViewModel()
        {
            BossSplit = new BossSplit();
        }


        public string Testyy { get; set; } = "Hosterd";

        private ISplit _split;
        public BossSplit BossSplit
        {
            get
            {
                if (SplitType == SplitType.Boss)
                {
                    return (BossSplit)_split;
                }
                return null;
            }
            set
            {
                _split = value;
                OnPropertyChanged();
            }
        }




        private SplitType _splitType;
        public SplitType SplitType
        {
            get => _splitType;
            set
            {
                _splitType = value;
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
