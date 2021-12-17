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
            foreach (var bonfireHub in DS2SBonfireHub.All)
            {
                cmbBonfirHub.Items.Add(bonfireHub);
            }
        }

        internal override void UpdateCtrl() 
        {
            Hook.UpdateBonfireProperties();
        }

        internal override void EnableCtrls(bool enable) 
        {
            IsEnabled = enable;
        }
        internal override void ReloadCtrl()
        {
            cmbBonfirHub.SelectedIndex = -1;
            cmbBonfirHub.SelectedIndex = 0;
        }
        private void UnlockBonfires_Click(object sender, RoutedEventArgs e)
        {
            Hook.UnlockBonfires();
        }

        private void cmbBonfirHub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Hook == null)
                return;

            spBonfires.Children.Clear();
            var bonfireHub = cmbBonfirHub.SelectedItem as DS2SBonfireHub;
            if (bonfireHub == null)
                return;

            foreach (var bonfire in bonfireHub.Bonfires)
            {
                var hookBonfire = Hook.GetType().GetProperty(bonfire.Replace(" ", "").Replace("'", ""));
                var bonfireControl = new BonfireControl();
                bonfireControl.BonfireName = bonfire;
                bonfireControl.BonfireLevel = (byte)hookBonfire.GetValue(Hook);
                bonfireControl.nudBonfireLevel.ValueChanged += nudChanged;
               
                spBonfires.Children.Add(bonfireControl);
            }
        }

        private void nudChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var nud = sender as Control;
            var grid = nud.Parent as Grid;
            var bonfireControl = grid.Parent as BonfireControl;
            var hookBonfire = Hook.GetType().GetProperty(bonfireControl.BonfireName.Replace(" ", "").Replace("'", ""));
            hookBonfire.SetValue(Hook, (byte)bonfireControl.nudBonfireLevel.Value.Value);
            bonfireControl.BonfireLevel = (byte)hookBonfire.GetValue(Hook);
        }
    }
}
