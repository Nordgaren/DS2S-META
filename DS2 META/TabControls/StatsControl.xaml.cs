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

        public StatsControl()
        {
            InitializeComponent();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //var val = (int)e.NewValue;
        }
    }
}
