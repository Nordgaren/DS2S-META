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
    /// Interaction logic for ItemControl.xaml
    /// </summary>
    public partial class ItemControl : METAControl
    {
        public ItemControl()
        {
            InitializeComponent();
        }

        public override void InitTab()
        {
            DS2SItemCategory.GetItemCategories();
            DS2SInfusion.BuildInfusionDicts();
            foreach (DS2SItemCategory category in DS2SItemCategory.All)
                cmbCategory.Items.Add(category);
            cmbCategory.SelectedIndex = 0;
            FilterItems();
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterItems();
        }

        //Clear items and add the ones that match text in search box
        private void FilterItems()
        {
            lbxItems.Items.Clear();

            if (SearchAllCheckbox.IsChecked.Value && txtSearch.Text != "")
            {
                //search every item category
                foreach (DS2SItemCategory category in cmbCategory.Items)
                {
                    foreach (DS2SItem item in category.Items)
                    {
                        if (item.ToString().ToLower().Contains(txtSearch.Text.ToLower()))
                            lbxItems.Items.Add(item);
                    }
                }
            }
            else
            {
                //only search selected item category
                DS2SItemCategory category = cmbCategory.SelectedItem as DS2SItemCategory;
                foreach (DS2SItem item in category.Items)
                {
                    if (item.ToString().ToLower().Contains(txtSearch.Text.ToLower()))
                        lbxItems.Items.Add(item);
                }
            }

            if (lbxItems.Items.Count > 0)
                lbxItems.SelectedIndex = 0;

            HandleSearchLabel();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterItems();
        }

        //Handles the "Searching..." label on the text box
        private void HandleSearchLabel()
        {
            if (txtSearch.Text == "")
                lblSearch.Visibility = Visibility.Visible;
            else
                lblSearch.Visibility = Visibility.Hidden;

        }

        private void cbxQuantityRestrict_Checked(object sender, RoutedEventArgs e)
        {
            if (lbxItems == null)
                return;

            if (!cbxQuantityRestrict.IsChecked.Value)
            {
                nudQuantity.IsEnabled = true;
                nudQuantity.Maximum = int.MaxValue;
            }
            else if (lbxItems.SelectedIndex != -1)
            {
                // Must Fix
                //DS2SItem item = lbxItems.SelectedItem as DS2SItem;
                //nudQuantity.Maximum = item.StackLimit;
                //if (item.StackLimit == 1)
                //    nudQuantity.IsEnabled = false;
            }
        }

        private void cmbInfusion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var infusion = cmbInfusion.SelectedItem as DS2SInfusion;
            //Checks if cbxMaxUpgrade is checked and sets the value to max value
            HandleMaxItemCheckbox();

        }

        private void lbxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!Hook.Loaded) return;

            DS2SItem item = lbxItems.SelectedItem as DS2SItem;
            if (item == null)
                return;

            if (cbxQuantityRestrict.IsChecked.Value)
            {
                nudQuantity.Maximum = Hook.GetMaxQuantity(item);
                nudQuantity.IsEnabled = nudQuantity.Maximum > 1;
            }

            cmbInfusion.Items.Clear();
            //foreach (var infusion in DS2SInfusion.InfusionDict[item.Infusion])
            //    cmbInfusion.Items.Add(infusion);
            //cmbInfusion.SelectedIndex = 0;
            //cmbInfusion.IsEnabled = cmbInfusion.Items.Count > 1;

            //nudUpgrade.Maximum = item.MaxUpgrade;
            //nudUpgrade.IsEnabled = item.MaxUpgrade > 0;
            if (item.Type == DS2SItem.ItemType.Weapon)
                foreach (var infusion in Hook.GetWeaponInfusions(item.ID))
                    cmbInfusion.Items.Add(infusion);
            else
                cmbInfusion.Items.Add(DS2SInfusion.Normal);
            
            cmbInfusion.SelectedIndex = 0;
            cmbInfusion.IsEnabled = cmbInfusion.Items.Count > 1;

            nudUpgrade.Maximum = Hook.GetMaxUpgrade(item);
            nudUpgrade.IsEnabled = nudUpgrade.Maximum > 0;

            HandleMaxItemCheckbox();
        }

        internal void ReloadCtrl() 
        {
            lbxItems.SelectedIndex = -1;
            lbxItems.SelectedIndex = 0;
        }

        internal void EnableStats(bool enable)
        {
            btnCreate.IsEnabled = enable;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            _ = ChangeColor(Brushes.DarkGray);
            CreateItem();
        }

        //I think this is for safety so you don't spawn two items (not my code) - Nord
        private void lbxItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _ = ChangeColor(Brushes.DarkGray);
            CreateItem();
        }

        //Apply hair to currently loaded character
        public void CreateItem()
        {
            //Check if the button is enabled and the selected item isn't null
            if (btnCreate.IsEnabled && lbxItems.SelectedItem != null)
            {
                _ = ChangeColor(Brushes.DarkGray);
                DS2SItem item = lbxItems.SelectedItem as DS2SItem;

                var id = item.ID;

                var infusion = cmbInfusion.SelectedItem as DS2SInfusion;

                Hook.GetItem(id, (short)nudQuantity.Value, (byte)nudUpgrade.Value, (byte)infusion.ID, Properties.Settings.Default.SilentItemGive);
            }
        }

        //handles up and down scrolling
        private void ScrollListbox(KeyEventArgs e)
        {
            //Scroll down through Items listbox and go back to bottom at end
            if (e.Key == Key.Up)
            {
                e.Handled = true;//Do not pass keypress along

                //One liner meme that does the exact same thing as the code above
                lbxItems.SelectedIndex = ((lbxItems.SelectedIndex - 1) + lbxItems.Items.Count) % lbxItems.Items.Count;
                lbxItems.ScrollIntoView(lbxItems.SelectedItem);
                return;
            }

            //Scroll down through Items listbox and go back to top at end
            if (e.Key == Key.Down)
            {
                e.Handled = true;//Do not pass keypress along

                //One liner meme that does the exact same thing as the code above
                lbxItems.SelectedIndex = (lbxItems.SelectedIndex + 1) % lbxItems.Items.Count;
                lbxItems.ScrollIntoView(lbxItems.SelectedItem);
                return;
            }
        }

        //Changes the color of the Apply button
        private async Task ChangeColor(Brush new_color)
        {
            btnCreate.Background = new_color;

            await Task.Delay(TimeSpan.FromSeconds(.25));

            btnCreate.Background = default(Brush);
        }

        //handles escape
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                txtSearch.Clear();
                return;
            }

            //Create selected index as item
            if (e.Key == Key.Enter)
            {
                e.Handled = true; //Do not pass keypress along
                CreateItem();
                return;
            }

            //Return if sender is cmbInfusion so that arrow Key are handled correctly
            if (sender == cmbInfusion)
                return;
            //Prevents up and down Key from moving the cursor left and right when nothing in item box
            if (lbxItems.Items.Count == 0)
            {
                if (e.Key == Key.Up)
                    e.Handled = true; //Do not pass keypress along
                if (e.Key == Key.Down)
                    e.Handled = true; //Do not pass keypress along
                return;
            }

            ScrollListbox(e);
        }

        //Select number in nud
        private void nudUpgrade_Click(object sender, EventArgs e)
        {
            nudUpgrade.Focus();
        }

        private void SearchAllCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //checkbox changed, refresh search filter (if txtSearch is not empty)
            if (txtSearch.Text != "")
                FilterItems();
        }

        private void cbxMaxUpgrade_Checked(object sender, RoutedEventArgs e)
        {
            //HandleMaxItemCheckbox()
            if (cbxMaxUpgrade.IsChecked.Value)
            {
                nudUpgrade.Value = nudUpgrade.Maximum;
                nudQuantity.Value = nudQuantity.Maximum;
            }
            else
            {
                nudUpgrade.Value = nudUpgrade.Minimum;
                nudQuantity.Value = nudQuantity.Minimum;
            }
        }

        private void HandleMaxItemCheckbox()
        {
            //Set upgrade nud to max if max checkbox is ticked
            if (cbxMaxUpgrade.IsChecked.Value)
            {
                nudUpgrade.Value = nudUpgrade.Maximum;
                nudQuantity.Value = nudQuantity.Maximum;
            }
        }

        private void cmbInfusion_KeyDown(object sender, KeyEventArgs e)
        {
            //Create selected index as item
            if (e.Key == Key.Enter)
            {
                e.Handled = true; //Do not pass keypress along
                CreateItem();
                return;
            }
        }
        //Select all text in search box
        private void txtSearch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.SelectAll();
            txtSearch.Focus();
        }

    }
}
