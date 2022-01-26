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
    /// Interaction logic for CovenantControl.xaml
    /// </summary>
    public partial class CovenantControl : METAControl
    {
        public CovenantControl()
        {
            InitializeComponent();
            foreach (var covenant in DS2SCovenant.All)
            {
                cmbCovenant.Items.Add(covenant);
            }
            cmbCovenant.SelectedIndex = 0;
        }
        internal override void EnableCtrls(bool enable)
        {
            spCovenants.IsEnabled = enable;
            btnSet.IsEnabled = enable;
        }
        private void cmbCovenant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Hook == null)
                return;

            spCovenants.Children.Clear();
            var covenant = cmbCovenant.SelectedItem as DS2SCovenant;
            if (covenant == null)
                return;

            if (covenant.ID == 0)
                return;

            var covenantCheckbox = new CheckBox();
            Binding binding = new Binding("Value")
            {
                Source = Hook,
                Path = new PropertyPath($"{covenant.Name.Replace(" ", "")}Discovered")
            };
            covenantCheckbox.SetBinding(CheckBox.IsCheckedProperty, binding);
            covenantCheckbox.Content = $"{covenant.Name} Discovered";
            covenantCheckbox.HorizontalAlignment = HorizontalAlignment.Center;
            spCovenants.Children.Add(covenantCheckbox);

            var covenantControl = new LabelNudControl();
            binding = new Binding("Value")
            {
                Source = Hook,
                Path = new PropertyPath($"{covenant.Name.Replace(" ", "")}Rank")
            };
            covenantControl.nudValue.SetBinding(Xceed.Wpf.Toolkit.IntegerUpDown.ValueProperty, binding);
            covenantControl.nudValue.Minimum = 0;
            covenantControl.nudValue.Maximum = 3;
            covenantControl.Label = $"{covenant.Name} Rank";
            covenantControl.nudValue.Margin = new Thickness(0,5,0,0);
            spCovenants.Children.Add(covenantControl);

            covenantControl = new LabelNudControl();
            binding = new Binding("Value")
            {
                Source = Hook,
                Path = new PropertyPath($"{covenant.Name.Replace(" ", "")}Progress")
            };
            covenantControl.nudValue.SetBinding(Xceed.Wpf.Toolkit.IntegerUpDown.ValueProperty, binding);
            covenantControl.nudValue.Minimum = 0;
            var max = covenant.Levels.Split('/');
            covenantControl.nudValue.Maximum = int.Parse(max[2]);
            covenantControl.Label = $"{covenant.Name} Progress {covenant.Levels}";
            covenantControl.nudValue.Margin = new Thickness(0, 5, 0, 0);
            spCovenants.Children.Add(covenantControl);
        }

        private void SetCovenant_Click(object sender, RoutedEventArgs e)
        {
            var covenant = cmbCovenant.SelectedItem as DS2SCovenant;
            if (covenant == null)
                return;

            Hook.CurrentCovenant = covenant.ID;
        }
    }
}
