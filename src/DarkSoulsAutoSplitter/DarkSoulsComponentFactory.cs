using System;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using DarkSoulsAutoSplitter;


[assembly: ComponentFactory(typeof(DarkSoulsComponentFactory))]

namespace DarkSoulsAutoSplitter
{
    public class DarkSoulsComponentFactory : IComponentFactory
    {
        public string ComponentName => DarkSoulsComponent.Name;

        public string Description => "Configurable autosplitter for Dark Soul 1 ptde/community/remastered and Dark Souls 2/Scholar of the first sin";

        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => DarkSoulsComponent.Name;

        public string XMLURL => $"{UpdateURL}/Components/Updates.xml";

        public string UpdateURL => "https://github.com/Nordgaren/DS2S-META/tree/wasted/src/LiveSplit.DarkSouls2";

        public Version Version => new Version(0, 0, 1);

        public IComponent Create(LiveSplitState state)
        {
            return new DarkSoulsComponent(state);
        }
    }
}
