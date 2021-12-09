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
using Xceed.Wpf.Toolkit;

namespace DS2_META
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class TestControl : UserControl
    {
        public TestControl()
        {
            InitializeComponent();
        }

        public event EventHandler TestControlValueChanged;



        public int Vit
        {
            get { return (int)GetValue(VitProperty); }
            set { SetValue(VitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VitProperty =
            DependencyProperty.Register("Vit", typeof(int), typeof(TestControl), new PropertyMetadata(default));


        private void Nud1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TestControlValueChanged != null)
                TestControlValueChanged(this, EventArgs.Empty);
        }
    }
}
