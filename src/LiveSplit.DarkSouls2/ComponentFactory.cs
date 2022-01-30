using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.Model;
using LiveSplit.UI.Components;

namespace LiveSplit.DarkSouls2
{
    public class ComponentFactory : IComponentFactory
    {
        public string ComponentName => AutoSplitterComponent.Name;

        public string Description => "Configurable autosplitter for Dark Souls 2 & Dark Souls 2 Scholar of the first sin";

        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => AutoSplitterComponent.Name;

        public string XMLURL => "";

        public string UpdateURL => "";

        public Version Version => new Version(0, 0, 1);

        public IComponent Create(LiveSplitState state)
        {
            return new AutoSplitterComponent(state);
        }
    }
}
