﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsMemory.DarkSouls2;
using LiveSplit.DarkSouls2.Splits;

namespace LiveSplit.DarkSouls2.UI
{
    public class SplitViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SplitType> SplitTypesItemsSource { get; set; } = new ObservableCollection<SplitType>(Enum.GetValues(typeof(SplitType)).Cast<SplitType>());
        public ObservableCollection<BossType> BossTypeItemsSource { get; set; } = new ObservableCollection<BossType>(Enum.GetValues(typeof(BossType)).Cast<BossType>());
        public ObservableCollection<TimingType> TimingTypeItemsSource { get; set; } = new ObservableCollection<TimingType>(Enum.GetValues(typeof(TimingType)).Cast<TimingType>());


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

        public ItemSplit ItemSplit
        {
            get
            {
                if (SplitType == SplitType.Item)
                {
                    return (ItemSplit)_split;
                }
                return null;
            }
            set
            {
                _split = value;
                OnPropertyChanged();
            }
        }

        public BoxSplit BoxSplit
        {
            get
            {
                if (SplitType == SplitType.Box)
                {
                    return (BoxSplit)_split;
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

                switch (_splitType)
                {
                    case SplitType.Boss:
                        BossSplit = new BossSplit();
                        break;

                    case SplitType.Item:
                        ItemSplit = new ItemSplit();
                        break;

                    case SplitType.Box:
                        BoxSplit = new BoxSplit();
                        break;
                }

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
