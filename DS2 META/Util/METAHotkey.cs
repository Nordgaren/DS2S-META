using LowLevelHooking;
using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace DS2_META
{
    class METAHotkey
    {
        private string SettingsName;
        private TextBox HotkeyTextBox;
        private TabItem HotkeyTabPage;
        private Action HotkeyAction;
        private Brush DefaultColor;

        public VirtualKey Key;

        public METAHotkey(string settingsName, TextBox setTextBox, TabItem setTabPage, Action setAction)
        {
            SettingsName = settingsName;
            HotkeyTextBox = setTextBox;
            DefaultColor = HotkeyTextBox.Background;
            HotkeyTabPage = setTabPage;
            HotkeyAction = setAction;

            Key = (VirtualKey)(int)Properties.Settings.Default[SettingsName];

            if (Key == VirtualKey.Escape)
                HotkeyTextBox.Text = "Unbound";
            else
                HotkeyTextBox.Text = Key.ToString();

            HotkeyTextBox.MouseEnter += HotkeyTextBox_MouseEnter;
            HotkeyTextBox.MouseLeave += HotkeyTextBox_MouseLeave;
            HotkeyTextBox.KeyUp += HotkeyTextBox_KeyUp;
        }

        private void HotkeyTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var parse = Enum.TryParse(e.Key.ToString(), out LowLevelHooking.VirtualKey virtualKey);
            if (!parse)
            {
                HotkeyTextBox.Text = "Unbound";
                return;
            }

            Key = virtualKey;
            if (Key == VirtualKey.Escape)
                HotkeyTextBox.Text = "Unbound";
            else
                HotkeyTextBox.Text = Key.ToString();
            e.Handled = true;
            HotkeyTabPage.Focus();
        }
        private void HotkeyTextBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HotkeyTextBox.Background = DefaultColor;
        }

        private void HotkeyTextBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HotkeyTextBox.Background = Brushes.LightGreen;
        }
        public bool Trigger(VirtualKey pressed)
        {
            bool result = false;
            if (Key != VirtualKey.Escape && pressed == Key)
            {
                HotkeyAction();
                result = true;
            }
            return result;
        }

        public void Save()
        {
            Properties.Settings.Default[SettingsName] = (int)Key;
        }
    }
}
