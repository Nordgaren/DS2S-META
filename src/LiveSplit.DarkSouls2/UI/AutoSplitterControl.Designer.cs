using System.Windows.Forms;

namespace LiveSplit.DarkSouls2
{
    partial class AutoSplitterControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.iSplitBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.mainControl1 = new LiveSplit.DarkSouls2.UI.MainControl();
            ((System.ComponentModel.ISupportInitialize)(this.iSplitBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // iSplitBindingSource
            // 
            this.iSplitBindingSource.DataSource = typeof(LiveSplit.DarkSouls2.Splits.ISplit);
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(370, 613);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.mainControl1;
            // 
            // AutoSplitterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost1);
            this.Name = "AutoSplitterControl";
            this.Size = new System.Drawing.Size(370, 613);
            ((System.ComponentModel.ISupportInitialize)(this.iSplitBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource iSplitBindingSource;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private UI.MainControl mainControl1;
    }
}
