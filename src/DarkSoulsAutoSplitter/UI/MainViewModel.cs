using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsAutoSplitter.Splits;
using DarkSoulsAutoSplitter.UI.DarkSouls1;
using DarkSoulsAutoSplitter.UI.ViewModel;

namespace DarkSoulsAutoSplitter.UI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public int SelectedGameIndex
        {
            get => _selectedGameIndex;
            set
            {
                _selectedGameIndex = value;
                OnPropertyChanged();
            }
        }
        private int _selectedGameIndex = 0;





        public DarkSouls1ViewModel DarkSouls1ViewModel
        {
            get => _darkSouls1ViewModel;
            set
            {
                _darkSouls1ViewModel = value;
                OnPropertyChanged();
            }
        }

        private DarkSouls1ViewModel _darkSouls1ViewModel = new DarkSouls1ViewModel();







        public DarkSouls2ViewModel DarkSouls2ViewModel
        {
            get => _darkSouls2ViewModel;
            set
            {
                _darkSouls2ViewModel = value;
                OnPropertyChanged();
            }
        }

        private DarkSouls2ViewModel _darkSouls2ViewModel = new DarkSouls2ViewModel();



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
