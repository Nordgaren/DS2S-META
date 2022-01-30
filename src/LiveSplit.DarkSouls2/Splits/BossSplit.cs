﻿using DarkSoulsMemory.DarkSouls1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls2.Splits
{
    public class BossSplit : ISplit
    {
        public BossSplit()
        {
            Testy = "Hosterd";
        }

        private string _testy = "Hosterd";

        public string Testy
        {
            get => _testy;
            set
            {
                _testy = value;
                OnPropertyChanged();
            }
        }

        public SplitType SplitType => SplitType.Boss;

        private BossType _bossType;
        public BossType BossType
        {
            get => _bossType;
            set
            {
                _bossType = value;
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
