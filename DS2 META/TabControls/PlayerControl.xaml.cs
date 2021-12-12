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
        float AngX = 0;
        float AngY = 0;
        float AngZ = 0;
        public void StorePosition()
        {
            if (btnPosStore.IsEnabled)
            {
                AngX = Hook.AngX;
                AngY = Hook.AngY;
                AngZ = Hook.AngZ;
                nudPosStoredX.Value = nudPosX.Value;
                nudPosStoredY.Value = nudPosY.Value;
                nudPosStoredZ.Value = nudPosZ.Value;
            }
        }
        public void RestorePosition()
        {
            if (btnPosRestore.IsEnabled)
            {
                Hook.AngX = AngX;
                Hook.AngY = AngY;
                Hook.AngZ = AngZ;
                Hook.PosWarp((float)nudPosStoredX.Value, (float)nudPosStoredY.Value, (float)nudPosStoredZ.Value);
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
            btnPosRestore.IsEnabled = enable;
            nudPosStoredX.IsEnabled = enable;
            nudPosStoredY.IsEnabled = enable;
            nudPosStoredZ.IsEnabled = enable;
            nudHealth.IsEnabled = enable;
            nudStamina.IsEnabled = enable;
            cbxSpeed.IsEnabled = enable;
            cbxGravity.IsEnabled = enable;
        }

        private void btnStore_Click(object sender, RoutedEventArgs e)
        {
            StorePosition();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            RestorePosition();
        }

        private void cbxSpeed_Checked(object sender, RoutedEventArgs e)
        {
            nudSpeed.IsEnabled = cbxSpeed.IsChecked.Value;
            Hook.Speed = cbxSpeed.IsChecked.Value ? (float)nudSpeed.Value : 1;
        }

        private void nudSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (GameLoaded)
                Hook.Speed = (float)nudSpeed.Value;
        }
    }
}
