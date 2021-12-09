using PropertyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2_META
{
    internal class DS2ViewModel : ObservableObject
    {
        public DS2Hook Hook { get; private set; }
        public bool Loaded { get; set; }

        public DS2ViewModel()
        {
            Hook = new DS2Hook(5000, 5000);
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
