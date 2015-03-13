namespace y86sim.UI
{
    partial class frmSettings
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
            this.ppgWatcher = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // ppgWatcher
            // 
            this.ppgWatcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppgWatcher.HelpVisible = false;
            this.ppgWatcher.Location = new System.Drawing.Point(0, 0);
            this.ppgWatcher.Name = "ppgWatcher";
            this.ppgWatcher.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ppgWatcher.Size = new System.Drawing.Size(284, 262);
            this.ppgWatcher.TabIndex = 5;
            this.ppgWatcher.ToolbarVisible = false;
            this.ppgWatcher.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ppgWatcher_PropertyValueChanged);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ppgWatcher);
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid ppgWatcher;
    }
}