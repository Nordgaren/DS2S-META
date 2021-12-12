using LowLevelHooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DS2_META
{
    public partial class MainWindow : Window
    {
        private GlobalKeyboardHook KeyboardHook = new GlobalKeyboardHook();
        private List<METAHotkey> Hotkeys = new List<METAHotkey>();

        private void InitHotkeys()
        {
            cbxEnableHotkeys.IsChecked = Settings.EnableHotkeys;
            cbxHandleHotkeys.IsChecked = Settings.HandleHotkeys;

            Hotkeys.Add(new METAHotkey("StorePosition", hkeyStorePosition.tbxHotkey, tabHotkeys, () =>
            {
                metaPlayer.StorePosition();
            }));

            Hotkeys.Add(new METAHotkey("RestorePosition", hkeyRestorePosition.tbxHotkey, tabHotkeys, () =>
            {
                metaPlayer.RestorePosition();
            }));

            KeyboardHook.KeyDownOrUp += GlobalKeyboardHook_KeyDownOrUp;
        }

        private void ResetHotkeys() { }

        private void SaveHotkeys()
        {
            Settings.EnableHotkeys = cbxEnableHotkeys.IsChecked.Value;
            Settings.HandleHotkeys = cbxHandleHotkeys.IsChecked.Value;
            foreach (METAHotkey hotkey in Hotkeys)
                hotkey.Save();
            KeyboardHook.Dispose();
        }

        private void ReloadHotkeys() { }
        private void UpdateHotkeys() { }

        private void GlobalKeyboardHook_KeyDownOrUp(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!e.IsUp && FormLoaded && cbxEnableHotkeys.IsChecked.Value && Hook.Focused)
            {
                foreach (METAHotkey hotkey in Hotkeys)
                {
                    if (hotkey.Trigger(e.KeyCode) && cbxEnableHotkeys.IsChecked.Value)
                        e.Handled = true;
                }
            }
        }
    }
}
