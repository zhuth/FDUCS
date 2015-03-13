namespace y86sim.UI
{
    partial class frmAsm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAsm));
            this.txtArray = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // txtArray
            // 
            this.txtArray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArray.Encoding = ((System.Text.Encoding)(resources.GetObject("txtArray.Encoding")));
            this.txtArray.Location = new System.Drawing.Point(0, 0);
            this.txtArray.Name = "txtArray";
            this.txtArray.ShowEOLMarkers = true;
            this.txtArray.ShowHRuler = true;
            this.txtArray.ShowInvalidLines = false;
            this.txtArray.ShowSpaces = true;
            this.txtArray.ShowTabs = true;
            this.txtArray.ShowVRuler = true;
            this.txtArray.Size = new System.Drawing.Size(674, 293);
            this.txtArray.TabIndent = 8;
            this.txtArray.TabIndex = 1;
            this.txtArray.Load += new System.EventHandler(this.txtArray_Load);
            this.txtArray.DoubleClick += new System.EventHandler(this.txtArray_DoubleClick);
            // 
            // frmAsm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 293);
            this.Controls.Add(this.txtArray);
            this.Name = "frmAsm";
            this.Text = "Assembly";
            this.Load += new System.EventHandler(this.frmAsm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControl txtArray;
    }
}