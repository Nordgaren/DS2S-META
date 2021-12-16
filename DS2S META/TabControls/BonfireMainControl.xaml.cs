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

namespace DS2S_META
{
    /// <summary>
    /// Interaction logic for BonfireMainControl.xaml
    /// </summary>
    public partial class BonfireMainControl : METAControl
    {
        public BonfireMainControl()
        {
            InitializeComponent();
        }

        internal override void UpdateCtrl() 
        {
            Hook.UpdateBonfireProperties();
        }

        internal override void EnableCtrls(bool enable) 
        {
            IsEnabled = enable;
        }

        private void UnlockBonfires_Click(object sender, RoutedEventArgs e)
        {
            Hook.UnlockBonfires();
        }
    }
}
