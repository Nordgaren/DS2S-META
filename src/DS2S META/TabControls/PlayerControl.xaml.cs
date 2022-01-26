﻿using System;
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

        public override void InitTab()
        {
            PlayerState.Set = false;
            foreach (var bonfire in DS2SBonfire.All)
                cbxBonfire.Items.Add(bonfire);
            LastSetBonfire = new DS2SBonfire(0, 0, "Last Set: None"); //last set bonfire (default values)
            cbxBonfire.Items.Add(LastSetBonfire); //add to end of filter
            Positions = SavedPos.GetSavedPositions();
            cmbStoredPositions.Items.Add(new SavedPos());
            UpdatePositions();
        }
        internal override void ReloadCtrl()
        {
            if (WarpRest)
                Hook.ApplySpecialEffect(110000010);
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
            cbxSpeed.IsEnabled = enable || Hook.Setup;
            cbxGravity.IsEnabled = enable;
            cbxCollision.IsEnabled = enable;
            btnWarp.IsEnabled = enable && !Hook.Multiplayer;
            cbxWarpRest.IsEnabled = enable;

            if (enable)
                cbxBonfire.SelectedIndex = cbxBonfire.Items.Count - 1;
        }
        public void StorePosition()
        {
            if (btnPosStore.IsEnabled)
            {
                var pos = new SavedPos();
                pos.Name = cmbStoredPositions.Text;
                nudPosStoredX.Value = (decimal)Hook.StableX;
                nudPosStoredY.Value = (decimal)Hook.StableY;
                nudPosStoredZ.Value = (decimal)Hook.StableZ;
                PlayerState.AngX = Hook.AngX;
                PlayerState.AngY = Hook.AngY;
                PlayerState.AngZ = Hook.AngZ;
                PlayerState.HP = (int)nudHealth.Value;
                PlayerState.Stamina = (int)nudStamina.Value;
                PlayerState.FollowCam = Hook.CameraData;
                PlayerState.FollowCam2 = Hook.CameraData2;
                PlayerState.Set = true;
                pos.X = Hook.StableX;
                pos.Y = Hook.StableY;
                pos.Z = Hook.StableZ;
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
            if (!string.IsNullOrWhiteSpace(cmbStoredPositions.Text))
            {
                if (Positions.Any(n => n.Name == cmbStoredPositions.Text))
                {
                    var old = Positions.Single(n => n.Name == cmbStoredPositions.Text);
                    Positions.Remove(old);
                    Positions.Add(pos);
                    return;
                }

                Positions.Add(pos);
            }
            else
            {
                cmbStoredPositions.Items[0] = pos;
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
                DeleteButton_Click(sender, e);
            }
        }
        private void UpdatePositions()
        {
            if (cmbStoredPositions.SelectedItem != new SavedPos())
            {
                var blank = cmbStoredPositions.Items[0] as SavedPos;
                if (blank == null)
                    blank = new SavedPos();

                cmbStoredPositions.Items.Clear();
                cmbStoredPositions.Items.Add(blank);
                foreach (var item in Positions)
                {
                    cmbStoredPositions.Items.Add(item);
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
                //Hook.CameraData = PlayerState.FollowCam;
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
            if (Positions.Any(n => n.Name == cmbStoredPositions.Text))
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this positon?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    var old = Positions.Single(n => n.Name == cmbStoredPositions.Text);
                    Positions.Remove(old);
                    cmbStoredPositions.SelectedIndex = 0;
                    UpdatePositions();
                    SavedPos.Save(Positions);
                }

            }

        }
        private DS2SBonfire LastSetBonfire;

        public bool WarpRest { get; private set; }

        internal override void UpdateCtrl() 
        {
            //manage unknown warps and current warps that are not in filter
            var bonfireID = Hook.LastBonfireID;

            if (LastSetBonfire.ID != bonfireID) // lastSetBonfire does not match game LastBonfire
            {
                //target warp is not in filter
                var result = DS2SBonfire.All.FirstOrDefault(b => b.ID == bonfireID); //check if warp is in bonfire resource
                if (result == null)
                {
                    //bonfire not in filter. Add to filter as unknown
                    result = new DS2SBonfire(Hook.LastBonfireAreaID ,bonfireID, $"Unknown {Hook.LastBonfireAreaID}: {bonfireID}");
                    DS2SBonfire.All.Add(result);
                    FilterBonfires();
                }

                //manage lastSetBonfire
                cbxBonfire.Items.Remove(LastSetBonfire); //remove from filter (if there)

                LastSetBonfire.AreaID = result.AreaID;
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
            Hook.Speedhack(cbxSpeed.IsChecked.Value);
        }

        private void nudSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (GameLoaded && Hook.Hooked)
                Hook.SetSpeed((float)nudSpeed.Value);
        }
        private void nudSpeed_LostFocus(object sender, RoutedEventArgs e)
        {
            nudSpeed_ValueChanged(null, null);
        }
        private void cbxBonfire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Hook.Loaded && cbxQuickSelectBonfire.IsChecked.Value)
            {
                var bonfire = cbxBonfire.SelectedItem as DS2SBonfire;
                if (bonfire == null)
                    return;

                Hook.LastBonfireID = bonfire.ID;
                Hook.LastBonfireAreaID = bonfire.AreaID;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveSavedPos();
        }

        private void storedPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var savedPos = cmbStoredPositions.SelectedItem as SavedPos;
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

        private void WarpButton_Click(object sender, RoutedEventArgs e)
        {
            Warp();
        }

        public void Warp()
        {
            _ = ChangeColor(Brushes.DarkGray);
            if (Hook.Multiplayer)
            {
                System.Windows.MessageBox.Show("Warning: Cannot warp while engaging in Multiplayer", "Multiplayer Warp Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var bonfire = cbxBonfire.SelectedItem as DS2SBonfire;
            if (bonfire.ID == 0 || bonfire.AreaID == 0 || bonfire == null)
                return;

            Hook.LastBonfireID = bonfire.ID;
            Hook.LastBonfireAreaID = bonfire.AreaID;
            var warped = Hook.Warp(bonfire.ID);
            if (warped && cbxWarpRest.IsChecked.Value)
                WarpRest = true; 
        }

        private async Task ChangeColor(Brush new_color)
        {
            btnWarp.Background = new_color;

            await Task.Delay(TimeSpan.FromSeconds(.25));

            btnWarp.Background = default(Brush);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterBonfires();
        }
        private void KeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                txtSearch.Clear();

            KeyDownListbox(e);
        }
        private void KeyDownListbox(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;

                if (cbxBonfire.SelectedIndex < cbxBonfire.Items.Count - 1)
                {
                    cbxBonfire.SelectedIndex += 1;
                    return;
                }
            }

            if (e.Key == Key.Up)
            {
                e.Handled = true;

                if (cbxBonfire.SelectedIndex != 0)
                {
                    cbxBonfire.SelectedIndex -= 1;
                    return;
                }
            }

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Warp();
                return;
            }
        }
        private void txtSearch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.SelectAll();
            txtSearch.Focus();
            e.Handled=true;
        }
    }
}
