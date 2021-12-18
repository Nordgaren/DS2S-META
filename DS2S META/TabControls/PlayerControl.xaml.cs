using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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

        private State.PlayerState PlayerState;

        private List<SavedPos> Positions = new List<SavedPos>();

        float CamX;
        float CamY;
        float CamZ;
        public override void InitTab()
        {
            PlayerState.Set = false;
            foreach (var bonfire in DS2SBonfire.All)
                cbxBonfire.Items.Add(bonfire);
            LastSetBonfire = new DS2SBonfire(-1, "Last Set: None"); //last set bonfire (default values)
            cbxBonfire.Items.Add(LastSetBonfire); //add to end of filter
            Positions = SavedPos.GetSavedPositions();
            UpdatePositions();
        }
        internal override void ReloadCtrl()
        {
            if (cbxSpeed.IsChecked.Value)
            Hook.Speed = (float)nudSpeed.Value;
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
            cbxCollision.IsEnabled = enable;

            if (enable)
                cbxBonfire.SelectedIndex = cbxBonfire.Items.Count - 1;
        }
        public void StorePosition()
        {
            if (btnPosStore.IsEnabled)
            {
                var pos = new SavedPos();
                pos.Name = storedPositions.Text;
                nudPosStoredX.Value = (decimal)Hook.PosX;
                nudPosStoredY.Value = (decimal)Hook.PosY;
                nudPosStoredZ.Value = (decimal)Hook.PosZ;
                PlayerState.AngX = Hook.AngX;
                PlayerState.AngY = Hook.AngY;
                PlayerState.AngZ = Hook.AngZ;
                PlayerState.HP = (int)nudHealth.Value;
                PlayerState.Stamina = (int)nudStamina.Value;
                PlayerState.FollowCam = Hook.CameraData;
                PlayerState.Set = true;
                CamX = Hook.CamX;
                CamY = Hook.CamY;
                CamZ = Hook.CamZ;
                pos.X = Hook.PosX;
                pos.Y = Hook.PosY;
                pos.Z = Hook.PosZ;
                pos.PlayerState = PlayerState;
                ProcessSavedPos(pos);
                UpdatePositions();
                SavedPos.Save(Positions);
                
                txtAngX.Text = PlayerState.AngX.ToString("N2");
                txtAngY.Text = PlayerState.AngY.ToString("N2");
                txtAngZ.Text = PlayerState.AngZ.ToString("N2");
            }
        }
        public void ProcessSavedPos(SavedPos pos)
        {
            if (!string.IsNullOrWhiteSpace(storedPositions.Text))
            {
                if (Positions.Any(n => n.Name == storedPositions.Text))
                {
                    var old = Positions.Single(n => n.Name == storedPositions.Text);
                    Positions.Remove(old);
                    Positions.Add(pos);
                    return;
                }

                Positions.Add(pos);
            }

        }
        private void storedPositions_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StorePosition();
            }

            var shift = (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift));

            if (e.Key == Key.Delete && shift)
            {
                deleteButton_Click(sender, e);
            }
        }
        private void UpdatePositions()
        {
            if (storedPositions.SelectedItem != new SavedPos())
            {
                storedPositions.Items.Clear();
                storedPositions.Items.Add(new SavedPos());
                foreach (var item in Positions)
                {
                    storedPositions.Items.Add(item);
                }
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
                Hook.AngX = PlayerState.AngX;
                Hook.AngY = PlayerState.AngY;
                Hook.AngZ = PlayerState.AngZ;
                //Hook.CamX = CamX;
                //Hook.CamY = CamY;
                //Hook.CamZ = CamZ;
                if (cbxRestoreState.IsChecked.Value)
                {
                    nudHealth.Value = PlayerState.HP;
                    nudStamina.Value = PlayerState.Stamina;
                }
            }
        }

        public void RemoveSavedPos()
        {
            if (Positions.Any(n => n.Name == storedPositions.Text))
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this positon?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    var old = Positions.Single(n => n.Name == storedPositions.Text);
                    Positions.Remove(old);
                    storedPositions.SelectedIndex = 0;
                    UpdatePositions();
                    SavedPos.Save(Positions);
                }

            }

        }
        private DS2SBonfire LastSetBonfire;
        internal override void UpdateCtrl() 
        {
            //manage unknown warps and current warps that are not in filter
            int bonfireID = Hook.LastBonfireID;

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

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveSavedPos();
        }

        private void storedPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var savedPos = storedPositions.SelectedItem as SavedPos;
            if (savedPos == null)
                return;

            nudPosStoredX.Value = (decimal)savedPos.X;
            nudPosStoredY.Value = (decimal)savedPos.Y;
            nudPosStoredZ.Value = (decimal)savedPos.Z;
            PlayerState = savedPos.PlayerState;
            txtAngX.Text = PlayerState.AngX.ToString("N2");
            txtAngY.Text = PlayerState.AngY.ToString("N2");
            txtAngZ.Text = PlayerState.AngZ.ToString("N2");
        }
    }
}
