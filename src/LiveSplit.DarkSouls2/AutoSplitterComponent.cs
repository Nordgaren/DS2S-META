using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LiveSplit.DarkSouls2
{
    public class AutoSplitterComponent : IComponent
    {
        public const string Name = "Dark Souls 2 Autosplitter";

        public string ComponentName => Name;

        public float HorizontalWidth => throw new NotImplementedException();

        public float MinimumHeight => throw new NotImplementedException();

        public float VerticalHeight => throw new NotImplementedException();

        public float MinimumWidth => throw new NotImplementedException();

        public float PaddingTop => throw new NotImplementedException();

        public float PaddingBottom => throw new NotImplementedException();

        public float PaddingLeft => throw new NotImplementedException();

        public float PaddingRight => throw new NotImplementedException();

        public IDictionary<string, Action> ContextMenuControls => throw new NotImplementedException();


        public AutoSplitterComponent(LiveSplitState state = null)
        {

        }



        #region Xml settings ==============================================================================================================
        public XmlNode GetSettings(XmlDocument document)
        {
            return null;
            //throw new NotImplementedException();
        }

        public void SetSettings(XmlNode settings)
        {
            //throw new NotImplementedException();
        }

        #endregion




        public System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            throw new NotImplementedException();
        }

        

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            //throw new NotImplementedException();
        }




        #region unused ===================================================================================================================
        public void DrawHorizontal(System.Drawing.Graphics g, LiveSplitState state, float height, System.Drawing.Region clipRegion) { }
        public void DrawVertical(System.Drawing.Graphics g, LiveSplitState state, float width, System.Drawing.Region clipRegion) { }
        public void Dispose() { }

        #endregion

    }
}
