using System;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using LiveSplit.DarkSouls2;


[assembly: ComponentFactory(typeof(DarkSouls2ComponentFactory))]

namespace LiveSplit.DarkSouls2
{
    public class DarkSouls2ComponentFactory : IComponentFactory
    {
        public string ComponentName => DarkSouls2Component.Name;

        public string Description => "Configurable autosplitter for Dark Souls 2 & Dark Souls 2 Scholar of the first sin";

        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => DarkSouls2Component.Name;

        public string XMLURL => $"{UpdateURL}/Components/Updates.xml";

        public string UpdateURL => "https://github.com/Nordgaren/DS2S-META/tree/wasted/src/LiveSplit.DarkSouls2";

        public Version Version => new Version(0, 0, 1);

        public IComponent Create(LiveSplitState state)
        {
            return new DarkSouls2Component(state);
        }
    }
}
