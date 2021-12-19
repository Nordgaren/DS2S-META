using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for StatsControl.xaml
    /// </summary>
    public partial class StatsControl : METAControl
    {
        public StatsControl()
        {
            InitializeComponent();
            foreach (DS2SClass charClass in DS2SClass.All)
                cmbClass.Items.Add(charClass);
            cmbClass.SelectedIndex = -1;
        }
        public void ReloadTab()
        {

        }
        private void cbmClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Hook.Loaded)
            {
                DS2SClass charClass = cmbClass.SelectedItem as DS2SClass;
                Hook.Class = charClass.ID;
                nudVig.Minimum = charClass.Vigor;
                nudEnd.Minimum = charClass.Endurance;
                nudVit.Minimum = charClass.Vitality;
                nudAtt.Minimum = charClass.Attunement;
                nudStr.Minimum = charClass.Strength;
                nudDex.Minimum = charClass.Dexterity;
                nudAdp.Minimum = charClass.Adaptability;
                nudInt.Minimum = charClass.Intelligence;
                nudFth.Minimum = charClass.Faith;
            }
        }

        internal override void UpdateCtrl()
        {
            
        }

        internal override void ReloadCtrl()
        {
            cmbClass.SelectedItem = cmbClass.Items.Cast<DS2SClass>().FirstOrDefault(c => c.ID == Hook.Class);
            txtName.Text = Hook.Name;
        }

        internal override void EnableCtrls(bool enable)
        {
            cmbClass.IsEnabled = enable;
            txtName.IsEnabled = enable;
            btnGive.IsEnabled = enable;
            btnResetSoulMemory.IsEnabled = enable;
            nudGiveSouls.IsEnabled = enable;
            nudVig.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudEnd.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudVit.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudAtt.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudStr.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudDex.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudAdp.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudInt.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudFth.IsEnabled = enable && Properties.Settings.Default.EditStats;
            nudHollowLevel.IsEnabled = enable;

            if (!enable)
                cmbClass.SelectedIndex = -1;
        }
        private void GiveSouls_Click(object sender, RoutedEventArgs e)
        {
            if (nudGiveSouls.Value.HasValue)
                Hook.GiveSouls(nudGiveSouls.Value.Value);
        }
        private void ResetSoulMemory_Click(object sender, RoutedEventArgs e)
        {
            Hook.ResetSoulMemory();
        }
        private void Name_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Hook.Name = txtName.Text;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            nudVig.Value = nudVig.Minimum;
            nudEnd.Value = nudEnd.Minimum;
            nudVit.Value = nudVit.Minimum;
            nudAtt.Value = nudAtt.Minimum;
            nudStr.Value = nudStr.Minimum;
            nudDex.Value = nudDex.Minimum;
            nudAdp.Value = nudAdp.Minimum;
            nudInt.Value = nudInt.Minimum;
            nudFth.Value = nudFth.Minimum;
        }

        private void Max_Click(object sender, RoutedEventArgs e)
        {
            nudVig.Value = 99;
            nudEnd.Value = 99;
            nudVit.Value = 99;
            nudAtt.Value = 99;
            nudStr.Value = 99;
            nudDex.Value = 99;
            nudAdp.Value = 99;
            nudInt.Value = 99;
            nudFth.Value = 99;
        }
    }
}
