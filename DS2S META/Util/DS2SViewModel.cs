using PropertyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META
{
    internal class DS2SViewModel : ObservableObject
    {
        public DS2SHook Hook { get; private set; }
        public bool GameLoaded { get; set; }
        public bool Reading
        {
            get => DS2SHook.Reading;
            set => DS2SHook.Reading = value;
        }

        public DS2SViewModel()
        {
            Hook = new DS2SHook(5000, 5000);
            Hook.OnHooked += Hook_OnHooked;
            Hook.OnUnhooked += Hook_OnUnhooked;
            Hook.Start();
        }

        private void Hook_OnHooked(object sender, PHEventArgs e)
        {
        }

        private void Hook_OnUnhooked(object sender, PHEventArgs e)
        {

        }
    }
}
