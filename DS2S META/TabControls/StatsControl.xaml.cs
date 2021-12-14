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
            DS2SClass charClass = cmbClass.SelectedItem as DS2SClass;
            if (Hook.Loaded)
            {
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
            nudVig.IsEnabled = enable;
            nudEnd.IsEnabled = enable;
            nudVit.IsEnabled = enable;
            nudAtt.IsEnabled = enable;
            nudStr.IsEnabled = enable;
            nudDex.IsEnabled = enable;
            nudAdp.IsEnabled = enable;
            nudInt.IsEnabled = enable;
            nudFth.IsEnabled = enable;

            var lol = StatsCon.LogicalChildren;

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

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
