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

namespace DS2_META
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : METAControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }

        public void StorePosition()
        {
            if (btnPosStore.IsEnabled)
            {
                nudPosStoredX.Value = nudPosX.Value;
                nudPosStoredY.Value = nudPosY.Value;
                nudPosStoredZ.Value = nudPosZ.Value;
            }

        }
        public void RestorePosition()
        {
            if (btnPosRestore.IsEnabled)
            {
                Hook.PosX = (float)nudPosStoredX.Value;
                Hook.PosY = (float)nudPosStoredY.Value;
                Hook.PosZ = (float)nudPosStoredZ.Value;
            }
        }
        internal override void UpdateCtrl() 
        { 
        }
        internal override void ReloadCtrl() 
        { 
        }
        internal override void EnableCtrls(bool enable)
        {
            btnPosStore.IsEnabled = enable;
        }

        private void btnPosStore_Click(object sender, RoutedEventArgs e)
        {
            StorePosition();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RestorePosition();
        }
    }
}
