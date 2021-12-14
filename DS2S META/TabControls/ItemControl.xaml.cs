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
            /*
            lbxItems.Items.Clear();
            DS2SItemCategory category = cmbCategory.SelectedItem as DS2SItemCategory;
            foreach (DS2SItem item in category.Items)
                lbxItems.Items.Add(item);
            lbxItems.SelectedIndex = 0;
            txtSearch.Text = "";
            lblSearch.Visible = true;
            */
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


            /*
            //original code
            DS2SItemCategory category = cmbCategory.SelectedItem as DS2SItemCategory;
            foreach (DS2SItem item in category.Items)
            {
                if (item.ToString().ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    lbxItems.Items.Add(item);
                }
            }
            */

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
                DS2SItem item = lbxItems.SelectedItem as DS2SItem;
                nudQuantity.Maximum = item.StackLimit;
                if (item.StackLimit == 1)
                    nudQuantity.IsEnabled = false;
            }
        }

        private void cmbInfusion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var infusion = cmbInfusion.SelectedItem as DS2SInfusion;
            nudUpgrade.Maximum = infusion.MaxUpgrade;
            //Checks if cbxMaxUpgrade is checked and sets the value to max value
            HandleMaxItemCheckbox();

        }

        private void lbxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DS2SItem item = lbxItems.SelectedItem as DS2SItem;
            if (item == null)
                return;

            if (cbxQuantityRestrict.IsChecked.Value)
            {
                if (item.StackLimit <= 1)
                    nudQuantity.IsEnabled = false;
                else
                    nudQuantity.IsEnabled = true;
                nudQuantity.Maximum = item.StackLimit;
            }

            cmbInfusion.Items.Clear();
            foreach (var infusion in DS2SInfusion.InfusionDict[item.Infusion])
                cmbInfusion.Items.Add(infusion);
            cmbInfusion.IsEnabled = cmbInfusion.Items.Count > 1;

            nudUpgrade.Maximum = item.MaxUpgrade;
            nudUpgrade.IsEnabled = item.MaxUpgrade > 0;

            //switch (item.UpgradeType)
            //{
            //    case DS2SItem.Upgrade.None:
            //        cmbInfusion.Items.Clear();
            //        cmbInfusion.IsEnabled = false;
            //        nudUpgrade.IsEnabled = false;
            //        nudUpgrade.Maximum = 0;
            //        break;
            //    case DS2SItem.Upgrade.Armor:
            //        cmbInfusion.Items.Clear();
            //        cmbInfusion.IsEnabled = false;
            //        nudUpgrade.Maximum = 10;
            //        nudUpgrade.IsEnabled = true;
            //        break;
            //    case DS2SItem.Upgrade.InfusableFive:
            //        cmbInfusion.Items.Clear();
            //        nudUpgrade.Maximum = 5;
            //        foreach (var infusion in DS2SInfusion.All)
            //            cmbInfusion.Items.Add(infusion);
            //        cmbInfusion.SelectedIndex = 0;
            //        cmbInfusion.IsEnabled = true;
            //        nudUpgrade.IsEnabled = true;
            //        break;
            //    case DS2SItem.Upgrade.InfusableTen:
            //        cmbInfusion.Items.Clear();
            //        nudUpgrade.Maximum = 10;
            //        foreach (var infusion in DS2SInfusion.All)
            //            cmbInfusion.Items.Add(infusion);
            //        cmbInfusion.IsEnabled = true;
            //        nudUpgrade.IsEnabled = true;
            //        break;
            //    case DS2SItem.Upgrade.Shield:
            //        cmbInfusion.Items.Clear();
            //        nudUpgrade.Maximum = 10;
            //        foreach (var infusion in DS2SInfusion.All)
            //            if (infusion.Shield)
            //                cmbInfusion.Items.Add(infusion);
            //        cmbInfusion.SelectedIndex = 0;
            //        cmbInfusion.IsEnabled = true;
            //        nudUpgrade.IsEnabled = true;
            //        break;
            //    case DS2SItem.Upgrade.PyroFlame:
            //        cmbInfusion.IsEnabled = false;
            //        cmbInfusion.Items.Clear();
            //        nudUpgrade.Maximum = 10;
            //        nudUpgrade.IsEnabled = true;
            //        break;
            //}

            HandleMaxItemCheckbox();
        }

        internal void EnableStats(bool enable)
        {
            btnCreate.IsEnabled = enable;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            _ = ChangeColor(Brushes.DarkGray);
            CreateItem();

            //Mule Meme
            //foreach (DS2SItemCategory category in cmbCategory.Items)
            //{
            //    cmbCategory.SelectedItem = category;

            //    foreach (DS2SItem item in category.Items)
            //    {
            //        lbxItems.SelectedItem = item;
            //        int id = item.ID;
            //        if (item.UpgradeType == DS2SItem.Upgrade.PyroFlame || item.UpgradeType == DS2SItem.Upgrade.PyroFlameAscended)
            //            id += (int)nudUpgrade.Value * 100;
            //        else
            //            id += (int)nudUpgrade.Value;
            //        if (item.UpgradeType == DS2SItem.Upgrade.Infusable || item.UpgradeType == DS2SItem.Upgrade.InfusableRestricted)
            //        {
            //            DSInfusion infusion = cmbInfusion.SelectedItem as DSInfusion;
            //            id += infusion.Value;
            //        }
            //        Hook.GetItem(item.CategoryID, id, (int)nudQuantity.Value);
            //    }
            //}
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

                byte infusionID = 0;

                if (infusion != null)
                    infusionID = (byte)infusion.ID;

                
                Hook.GetItem(id, (short)nudQuantity.Value, (byte)nudUpgrade.Value, infusionID, Properties.Settings.Default.SilentItemGive);
            }
        }

        //handles up and down scrolling
        private void ScrollListbox(KeyEventArgs e)
        {
            //Scroll down through Items listbox and go back to bottom at end
            if (e.Key == Key.Up)
            {
                e.Handled = true;//Do not pass keypress along
                //Check is there's still items to go through
                if (lbxItems.SelectedIndex > 0)
                {
                    lbxItems.SelectedIndex -= 1;
                    lbxItems.ScrollIntoView(lbxItems.SelectedItem);
                    return;
                }

                //Check if last item or "over" for safety
                if (lbxItems.SelectedIndex <= 0)
                {
                    lbxItems.SelectedIndex = lbxItems.Items.Count - 1; //-1 because Selected Index is 0 based and Count isn't
                    lbxItems.ScrollIntoView(lbxItems.SelectedItem);
                    return;
                }

                //One liner meme that does the exact same thing as the code above
                //lbxItems.SelectedIndex = ((lbxItems.SelectedIndex - 1) + lbxItems.Items.Count) % lbxItems.Items.Count;
                //return;
            }

            //Scroll down through Items listbox and go back to top at end
            if (e.Key == Key.Down)
            {
                e.Handled = true;//Do not pass keypress along
                //Check is there's still items to go through
                if (lbxItems.SelectedIndex < lbxItems.Items.Count - 1) //-1 because Selected Index is 0 based and Count isn't
                {
                    lbxItems.SelectedIndex += 1;
                    lbxItems.ScrollIntoView(lbxItems.SelectedItem);
                    return;
                }

                //Check if last item or "over" for safety
                if (lbxItems.SelectedIndex >= lbxItems.Items.Count - 1) //-1 because Selected Index is 0 based and Count isn't
                {
                    lbxItems.SelectedIndex = 0;
                    lbxItems.ScrollIntoView(lbxItems.SelectedItem);
                    return;
                }

                //One liner meme that does the exact same thing as the code above
                //lbxItems.SelectedIndex = (lbxItems.SelectedIndex + 1) % lbxItems.Items.Count;
                //return;

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
