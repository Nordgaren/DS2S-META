using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveSplit.DarkSouls2.Splits;

namespace LiveSplit.DarkSouls2.UI
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();

            _mainViewModel = (MainViewModel)DataContext;
        }

        private MainViewModel _mainViewModel;

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.Splits.Add(new SplitViewModel());
        }
        
    }
}
