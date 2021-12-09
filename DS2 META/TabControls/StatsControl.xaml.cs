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
    /// Interaction logic for StatsControl.xaml
    /// </summary>
    public partial class StatsControl : UserControl
    {
        public DS2Hook Hook
        {
            get { return (DS2Hook)GetValue(HookProperty); }
            set { SetValue(HookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hook.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HookProperty =
            DependencyProperty.Register("Hook", typeof(DS2Hook), typeof(StatsControl), new PropertyMetadata(default));

        public bool Loaded
        {
            get { return (bool)GetValue(LoadedProperty); }
            set { SetValue(LoadedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loaded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadedProperty =
            DependencyProperty.Register("Loaded", typeof(bool), typeof(StatsControl), new PropertyMetadata(default));

        public StatsControl()
        {
            InitializeComponent();
            foreach (DS2Class charClass in DS2Class.All)
                cmbClass.Items.Add(charClass);
            cmbClass.SelectedIndex = -1;
        }

        private void Name_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Hook.Name = Name.Text;
        }

        public void ReloadTab()
        {

        }
        private void cbmClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DS2Class charClass = cmbClass.SelectedItem as DS2Class;
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

        internal void Update()
        {
            
        }

        internal void Reload()
        {
            cmbClass.SelectedItem = cmbClass.Items.Cast<DS2Class>().FirstOrDefault(c => c.ID == Hook.Class);
        }

        internal void EnableStats(bool v)
        {
            cmbClass.SelectedIndex = -1;
        }
    }
}
