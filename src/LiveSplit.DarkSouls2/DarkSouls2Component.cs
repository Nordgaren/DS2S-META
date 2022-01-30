using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LiveSplit.DarkSouls2.UI;

namespace LiveSplit.DarkSouls2
{
    public class DarkSouls2Component : IComponent
    {
        public const string Name = "Dark Souls 2 Autosplitter";

        public IDictionary<string, Action> ContextMenuControls => null;

        public DarkSouls2Component(LiveSplitState state = null)
        {

        }

        #region Xml settings ==============================================================================================================
        public XmlNode GetSettings(XmlDocument document)
        {
            XmlElement root = document.CreateElement("Settings");
            return root;
            //throw new NotImplementedException();
        }

        public void SetSettings(XmlNode settings)
        {
            //throw new NotImplementedException();
        }

        private MainControlFormsWrapper _mainControlFormsWrapper = new MainControlFormsWrapper();
        public System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            return _mainControlFormsWrapper;
        }
        #endregion







        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            //throw new NotImplementedException();
        }



        #region unused ===================================================================================================================
        public void DrawHorizontal(System.Drawing.Graphics g, LiveSplitState state, float height, System.Drawing.Region clipRegion) { }
        public void DrawVertical(System.Drawing.Graphics g, LiveSplitState state, float width, System.Drawing.Region clipRegion) { }
        public string ComponentName => Name;
        public float HorizontalWidth => 0;
        public float MinimumHeight => 0;
        public float VerticalHeight => 0;
        public float MinimumWidth => 0;
        public float PaddingTop => 0;
        public float PaddingBottom => 0;
        public float PaddingLeft => 0;
        public float PaddingRight => 0;
        public void Dispose() { }
        #endregion

    }
}
