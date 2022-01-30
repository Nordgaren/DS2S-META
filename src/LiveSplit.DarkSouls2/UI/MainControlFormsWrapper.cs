using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace LiveSplit.DarkSouls2.UI
{
    public partial class MainControlFormsWrapper : UserControl
    {
        public MainControlFormsWrapper()
        {
            _mainControl = new MainControl();
            Width = (int)_mainControl.Width;
            Height = (int)_mainControl.Height;

            SuspendLayout();

            _elementHost = new ElementHost();
            _elementHost.Location = new System.Drawing.Point(0, 0);
            _elementHost.Name = "ElementHost";
            _elementHost.Size = new System.Drawing.Size(Width, Height);
            _elementHost.TabIndex = 0;
            _elementHost.Text = "ElementHost";
            _elementHost.Child = _mainControl;
            Controls.Add(_elementHost);

            ResumeLayout(false);
        }

        private ElementHost _elementHost;
        private UI.MainControl _mainControl;
    }
}
