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
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : METAControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }
        public override void InitTab()
        {
            foreach (var bonfire in DS2SBonfire.All)
                cbxBonfire.Items.Add(bonfire);
            LastSetBonfire = new DS2SBonfire(-1, "Last Set: None"); //last set bonfire (default values)
            cbxBonfire.Items.Add(LastSetBonfire); //add to end of filter
        }

        float AngX = 0;
        float AngY = 0;
        float AngZ = 0;
        public void StorePosition()
        {
            if (btnPosStore.IsEnabled)
            {
                nudPosStoredX.Value = nudPosX.Value;
                nudPosStoredY.Value = nudPosY.Value;
                nudPosStoredZ.Value = nudPosZ.Value;
                AngX = Hook.AngX;
                AngY = Hook.AngY;
                AngZ = Hook.AngZ;
            }
        }
        public void RestorePosition()
        {
            if (btnPosRestore.IsEnabled)
            {
                if (!nudPosStoredX.Value.HasValue || !nudPosStoredY.Value.HasValue || !nudPosStoredZ.Value.HasValue)
                    return;

                Hook.StableX = (float)nudPosStoredX.Value;
                Hook.StableY = (float)nudPosStoredY.Value;
                Hook.StableZ = (float)nudPosStoredZ.Value;
                Hook.AngX = AngX;
                Hook.AngY = AngY;
                Hook.AngZ = AngZ;
            }
        }
        private DS2SBonfire LastSetBonfire;
        internal override void UpdateCtrl() 
        {
            //manage unknown warps and current warps that are not in filter
            int bonfireID = Hook.LastBonfire;

            if (LastSetBonfire.ID != bonfireID) // lastSetBonfire does not match game LastBonfire
            {
                //target warp is not in filter
                var result = DS2SBonfire.All.FirstOrDefault(b => b.ID == bonfireID); //check if warp is in bonfire resource
                if (result == null)
                {
                    //bonfire not in filter. Add to filter as unknown
                    result = new DS2SBonfire(bonfireID, $"Unknown {bonfireID}");
                    DS2SBonfire.All.Add(result);
                    FilterBonfires();
                }

                //manage lastSetBonfire
                cbxBonfire.Items.Remove(LastSetBonfire); //remove from filter (if there)

                LastSetBonfire.ID = result.ID;
                LastSetBonfire.Name = "Last Set: " + result.Name;

                cbxBonfire.Items.Add(LastSetBonfire); //add to end of filter
                cbxBonfire.SelectedItem = LastSetBonfire;
                //AddLastSetBonfire();
            }
        }
        private void FilterBonfires()
        {
            //warp filter management

            cbxBonfire.Items.Clear();
            cbxBonfire.SelectedItem = null;

            //go through bonfire resource and add to filter
            foreach (var bonfire in DS2SBonfire.All)
            {
                if (bonfire.ToString().ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    cbxBonfire.Items.Add(bonfire);
                }
            }

            cbxBonfire.Items.Add(LastSetBonfire); //add lastSetBonfire to end of filter

            cbxBonfire.SelectedIndex = 0;

            if (txtSearch.Text == "")
                lblSearch.Visibility = Visibility.Visible;
            else
                lblSearch.Visibility = Visibility.Hidden;
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

            if (enable)
                cbxBonfire.SelectedIndex = cbxBonfire.Items.Count - 1;
        }
        public void ToggleGravity()
        {
            cbxGravity.IsChecked = !cbxGravity.IsChecked;
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

        private void cbxBonfire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (Hook.Loaded && cbxQuickSelectBonfire.Checked)
            //    Hook.LastBonfire = ((DS2SBonfire)cbxBonfire.SelectedItem).ID;
        }
    }
}
