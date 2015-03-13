namespace y86sim
{
    partial class frmMain
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
            this.btnStep = new System.Windows.Forms.Button();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ppgWatcher = new System.Windows.Forms.PropertyGrid();
            this.lblIcodeF = new System.Windows.Forms.Label();
            this.lblIcodeD = new System.Windows.Forms.Label();
            this.lblIcodeE = new System.Windows.Forms.Label();
            this.lblIcodeM = new System.Windows.Forms.Label();
            this.lblPredPC = new System.Windows.Forms.Label();
            this.lblEsrcB = new System.Windows.Forms.Label();
            this.lblMdstM = new System.Windows.Forms.Label();
            this.lblEsrcA = new System.Windows.Forms.Label();
            this.lblMdstE = new System.Windows.Forms.Label();
            this.lblEdstM = new System.Windows.Forms.Label();
            this.lblEdstE = new System.Windows.Forms.Label();
            this.lblEvalB = new System.Windows.Forms.Label();
            this.lblMvalA = new System.Windows.Forms.Label();
            this.lblWdstM = new System.Windows.Forms.Label();
            this.lblWdstE = new System.Windows.Forms.Label();
            this.lblEvalA = new System.Windows.Forms.Label();
            this.lblMvalE = new System.Windows.Forms.Label();
            this.lblDvalP = new System.Windows.Forms.Label();
            this.lblDvalC = new System.Windows.Forms.Label();
            this.lblDrB = new System.Windows.Forms.Label();
            this.lblDrA = new System.Windows.Forms.Label();
            this.lblCC = new System.Windows.Forms.Label();
            this.lblEvalC = new System.Windows.Forms.Label();
            this.lblMBCH = new System.Windows.Forms.Label();
            this.lblWvalM = new System.Windows.Forms.Label();
            this.lblWvalE = new System.Windows.Forms.Label();
            this.lblIcodeW = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdOpen = new System.Windows.Forms.OpenFileDialog();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(12, 5);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(75, 23);
            this.btnStep.TabIndex = 1;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Visible = false;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnStep);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(755, 749);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(755, 774);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ppgWatcher);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.splitContainer1.Panel2.Controls.Add(this.lblIcodeF);
            this.splitContainer1.Panel2.Controls.Add(this.lblIcodeD);
            this.splitContainer1.Panel2.Controls.Add(this.lblIcodeE);
            this.splitContainer1.Panel2.Controls.Add(this.lblIcodeM);
            this.splitContainer1.Panel2.Controls.Add(this.lblPredPC);
            this.splitContainer1.Panel2.Controls.Add(this.lblEsrcB);
            this.splitContainer1.Panel2.Controls.Add(this.lblMdstM);
            this.splitContainer1.Panel2.Controls.Add(this.lblEsrcA);
            this.splitContainer1.Panel2.Controls.Add(this.lblMdstE);
            this.splitContainer1.Panel2.Controls.Add(this.lblEdstM);
            this.splitContainer1.Panel2.Controls.Add(this.lblEdstE);
            this.splitContainer1.Panel2.Controls.Add(this.lblEvalB);
            this.splitContainer1.Panel2.Controls.Add(this.lblMvalA);
            this.splitContainer1.Panel2.Controls.Add(this.lblWdstM);
            this.splitContainer1.Panel2.Controls.Add(this.lblWdstE);
            this.splitContainer1.Panel2.Controls.Add(this.lblEvalA);
            this.splitContainer1.Panel2.Controls.Add(this.lblMvalE);
            this.splitContainer1.Panel2.Controls.Add(this.lblDvalP);
            this.splitContainer1.Panel2.Controls.Add(this.lblDvalC);
            this.splitContainer1.Panel2.Controls.Add(this.lblDrB);
            this.splitContainer1.Panel2.Controls.Add(this.lblDrA);
            this.splitContainer1.Panel2.Controls.Add(this.lblCC);
            this.splitContainer1.Panel2.Controls.Add(this.lblEvalC);
            this.splitContainer1.Panel2.Controls.Add(this.lblMBCH);
            this.splitContainer1.Panel2.Controls.Add(this.lblWvalM);
            this.splitContainer1.Panel2.Controls.Add(this.lblWvalE);
            this.splitContainer1.Panel2.Controls.Add(this.lblIcodeW);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Size = new System.Drawing.Size(755, 749);
            this.splitContainer1.SplitterDistance = 176;
            this.splitContainer1.TabIndex = 2;
            // 
            // ppgWatcher
            // 
            this.ppgWatcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppgWatcher.HelpVisible = false;
            this.ppgWatcher.Location = new System.Drawing.Point(0, 0);
            this.ppgWatcher.Name = "ppgWatcher";
            this.ppgWatcher.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ppgWatcher.Size = new System.Drawing.Size(176, 749);
            this.ppgWatcher.TabIndex = 4;
            this.ppgWatcher.ToolbarVisible = false;
            // 
            // lblIcodeF
            // 
            this.lblIcodeF.AutoSize = true;
            this.lblIcodeF.Location = new System.Drawing.Point(36, 782);
            this.lblIcodeF.Name = "lblIcodeF";
            this.lblIcodeF.Size = new System.Drawing.Size(29, 12);
            this.lblIcodeF.TabIndex = 4;
            this.lblIcodeF.Text = "====";
            // 
            // lblIcodeD
            // 
            this.lblIcodeD.AutoSize = true;
            this.lblIcodeD.Location = new System.Drawing.Point(34, 613);
            this.lblIcodeD.Name = "lblIcodeD";
            this.lblIcodeD.Size = new System.Drawing.Size(29, 12);
            this.lblIcodeD.TabIndex = 3;
            this.lblIcodeD.Text = "====";
            // 
            // lblIcodeE
            // 
            this.lblIcodeE.AutoSize = true;
            this.lblIcodeE.Location = new System.Drawing.Point(34, 378);
            this.lblIcodeE.Name = "lblIcodeE";
            this.lblIcodeE.Size = new System.Drawing.Size(29, 12);
            this.lblIcodeE.TabIndex = 2;
            this.lblIcodeE.Text = "====";
            // 
            // lblIcodeM
            // 
            this.lblIcodeM.AutoSize = true;
            this.lblIcodeM.Location = new System.Drawing.Point(34, 231);
            this.lblIcodeM.Name = "lblIcodeM";
            this.lblIcodeM.Size = new System.Drawing.Size(29, 12);
            this.lblIcodeM.TabIndex = 1;
            this.lblIcodeM.Text = "====";
            // 
            // lblPredPC
            // 
            this.lblPredPC.AutoSize = true;
            this.lblPredPC.Location = new System.Drawing.Point(148, 783);
            this.lblPredPC.Name = "lblPredPC";
            this.lblPredPC.Size = new System.Drawing.Size(29, 12);
            this.lblPredPC.TabIndex = 0;
            this.lblPredPC.Text = "====";
            // 
            // lblEsrcB
            // 
            this.lblEsrcB.AutoSize = true;
            this.lblEsrcB.Location = new System.Drawing.Point(498, 378);
            this.lblEsrcB.Name = "lblEsrcB";
            this.lblEsrcB.Size = new System.Drawing.Size(29, 12);
            this.lblEsrcB.TabIndex = 0;
            this.lblEsrcB.Text = "====";
            // 
            // lblMdstM
            // 
            this.lblMdstM.AutoSize = true;
            this.lblMdstM.Location = new System.Drawing.Point(427, 231);
            this.lblMdstM.Name = "lblMdstM";
            this.lblMdstM.Size = new System.Drawing.Size(29, 12);
            this.lblMdstM.TabIndex = 0;
            this.lblMdstM.Text = "====";
            // 
            // lblEsrcA
            // 
            this.lblEsrcA.AutoSize = true;
            this.lblEsrcA.Location = new System.Drawing.Point(463, 378);
            this.lblEsrcA.Name = "lblEsrcA";
            this.lblEsrcA.Size = new System.Drawing.Size(29, 12);
            this.lblEsrcA.TabIndex = 0;
            this.lblEsrcA.Text = "====";
            // 
            // lblMdstE
            // 
            this.lblMdstE.AutoSize = true;
            this.lblMdstE.Location = new System.Drawing.Point(392, 231);
            this.lblMdstE.Name = "lblMdstE";
            this.lblMdstE.Size = new System.Drawing.Size(29, 12);
            this.lblMdstE.TabIndex = 0;
            this.lblMdstE.Text = "====";
            // 
            // lblEdstM
            // 
            this.lblEdstM.AutoSize = true;
            this.lblEdstM.Location = new System.Drawing.Point(427, 378);
            this.lblEdstM.Name = "lblEdstM";
            this.lblEdstM.Size = new System.Drawing.Size(29, 12);
            this.lblEdstM.TabIndex = 0;
            this.lblEdstM.Text = "====";
            // 
            // lblEdstE
            // 
            this.lblEdstE.AutoSize = true;
            this.lblEdstE.Location = new System.Drawing.Point(392, 378);
            this.lblEdstE.Name = "lblEdstE";
            this.lblEdstE.Size = new System.Drawing.Size(29, 12);
            this.lblEdstE.TabIndex = 0;
            this.lblEdstE.Text = "====";
            // 
            // lblEvalB
            // 
            this.lblEvalB.AutoSize = true;
            this.lblEvalB.Location = new System.Drawing.Point(341, 378);
            this.lblEvalB.Name = "lblEvalB";
            this.lblEvalB.Size = new System.Drawing.Size(29, 12);
            this.lblEvalB.TabIndex = 0;
            this.lblEvalB.Text = "====";
            // 
            // lblMvalA
            // 
            this.lblMvalA.AutoSize = true;
            this.lblMvalA.Location = new System.Drawing.Point(305, 231);
            this.lblMvalA.Name = "lblMvalA";
            this.lblMvalA.Size = new System.Drawing.Size(29, 12);
            this.lblMvalA.TabIndex = 0;
            this.lblMvalA.Text = "====";
            // 
            // lblWdstM
            // 
            this.lblWdstM.AutoSize = true;
            this.lblWdstM.Location = new System.Drawing.Point(427, 77);
            this.lblWdstM.Name = "lblWdstM";
            this.lblWdstM.Size = new System.Drawing.Size(29, 12);
            this.lblWdstM.TabIndex = 0;
            this.lblWdstM.Text = "====";
            // 
            // lblWdstE
            // 
            this.lblWdstE.AutoSize = true;
            this.lblWdstE.Location = new System.Drawing.Point(392, 77);
            this.lblWdstE.Name = "lblWdstE";
            this.lblWdstE.Size = new System.Drawing.Size(29, 12);
            this.lblWdstE.TabIndex = 0;
            this.lblWdstE.Text = "====";
            // 
            // lblEvalA
            // 
            this.lblEvalA.AutoSize = true;
            this.lblEvalA.Location = new System.Drawing.Point(267, 378);
            this.lblEvalA.Name = "lblEvalA";
            this.lblEvalA.Size = new System.Drawing.Size(29, 12);
            this.lblEvalA.TabIndex = 0;
            this.lblEvalA.Text = "====";
            // 
            // lblMvalE
            // 
            this.lblMvalE.AutoSize = true;
            this.lblMvalE.Location = new System.Drawing.Point(230, 231);
            this.lblMvalE.Name = "lblMvalE";
            this.lblMvalE.Size = new System.Drawing.Size(29, 12);
            this.lblMvalE.TabIndex = 0;
            this.lblMvalE.Text = "====";
            // 
            // lblDvalP
            // 
            this.lblDvalP.AutoSize = true;
            this.lblDvalP.Location = new System.Drawing.Point(328, 613);
            this.lblDvalP.Name = "lblDvalP";
            this.lblDvalP.Size = new System.Drawing.Size(29, 12);
            this.lblDvalP.TabIndex = 0;
            this.lblDvalP.Text = "====";
            // 
            // lblDvalC
            // 
            this.lblDvalC.AutoSize = true;
            this.lblDvalC.Location = new System.Drawing.Point(230, 613);
            this.lblDvalC.Name = "lblDvalC";
            this.lblDvalC.Size = new System.Drawing.Size(29, 12);
            this.lblDvalC.TabIndex = 0;
            this.lblDvalC.Text = "====";
            // 
            // lblDrB
            // 
            this.lblDrB.AutoSize = true;
            this.lblDrB.Location = new System.Drawing.Point(183, 613);
            this.lblDrB.Name = "lblDrB";
            this.lblDrB.Size = new System.Drawing.Size(29, 12);
            this.lblDrB.TabIndex = 0;
            this.lblDrB.Text = "====";
            // 
            // lblDrA
            // 
            this.lblDrA.AutoSize = true;
            this.lblDrA.Location = new System.Drawing.Point(148, 613);
            this.lblDrA.Name = "lblDrA";
            this.lblDrA.Size = new System.Drawing.Size(29, 12);
            this.lblDrA.TabIndex = 0;
            this.lblDrA.Text = "====";
            // 
            // lblCC
            // 
            this.lblCC.AutoSize = true;
            this.lblCC.Location = new System.Drawing.Point(135, 286);
            this.lblCC.Name = "lblCC";
            this.lblCC.Size = new System.Drawing.Size(29, 12);
            this.lblCC.TabIndex = 0;
            this.lblCC.Text = "====";
            // 
            // lblEvalC
            // 
            this.lblEvalC.AutoSize = true;
            this.lblEvalC.Location = new System.Drawing.Point(208, 378);
            this.lblEvalC.Name = "lblEvalC";
            this.lblEvalC.Size = new System.Drawing.Size(29, 12);
            this.lblEvalC.TabIndex = 0;
            this.lblEvalC.Text = "====";
            // 
            // lblMBCH
            // 
            this.lblMBCH.AutoSize = true;
            this.lblMBCH.Location = new System.Drawing.Point(148, 231);
            this.lblMBCH.Name = "lblMBCH";
            this.lblMBCH.Size = new System.Drawing.Size(29, 12);
            this.lblMBCH.TabIndex = 0;
            this.lblMBCH.Text = "====";
            // 
            // lblWvalM
            // 
            this.lblWvalM.AutoSize = true;
            this.lblWvalM.Location = new System.Drawing.Point(305, 77);
            this.lblWvalM.Name = "lblWvalM";
            this.lblWvalM.Size = new System.Drawing.Size(29, 12);
            this.lblWvalM.TabIndex = 0;
            this.lblWvalM.Text = "====";
            // 
            // lblWvalE
            // 
            this.lblWvalE.AutoSize = true;
            this.lblWvalE.Location = new System.Drawing.Point(230, 77);
            this.lblWvalE.Name = "lblWvalE";
            this.lblWvalE.Size = new System.Drawing.Size(29, 12);
            this.lblWvalE.TabIndex = 0;
            this.lblWvalE.Text = "====";
            // 
            // lblIcodeW
            // 
            this.lblIcodeW.AutoSize = true;
            this.lblIcodeW.Location = new System.Drawing.Point(34, 77);
            this.lblIcodeW.Name = "lblIcodeW";
            this.lblIcodeW.Size = new System.Drawing.Size(29, 12);
            this.lblIcodeW.TabIndex = 0;
            this.lblIcodeW.Text = "====";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::y86sim.Properties.Resources.PIPE;
            this.pictureBox1.Location = new System.Drawing.Point(0, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(605, 815);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.runToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(755, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.openToolStripMenuItem1.Text = "&Open...";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.settingsToolStripMenuItem.Text = "&Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(128, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem1,
            this.stepToolStripMenuItem,
            this.pauseToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(42, 21);
            this.runToolStripMenuItem.Text = "&Run";
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.ShortcutKeyDisplayString = "F5";
            this.runToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.runToolStripMenuItem1.Text = "&Run";
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.runToolStripMenuItem1_Click);
            // 
            // stepToolStripMenuItem
            // 
            this.stepToolStripMenuItem.Name = "stepToolStripMenuItem";
            this.stepToolStripMenuItem.ShortcutKeyDisplayString = "F10";
            this.stepToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.stepToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stepToolStripMenuItem.Text = "&Step";
            this.stepToolStripMenuItem.Click += new System.EventHandler(this.stepToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registersToolStripMenuItem,
            this.memoryToolStripMenuItem,
            this.assemblyToolStripMenuItem,
            this.stackToolStripMenuItem,
            this.instantToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(67, 21);
            this.windowToolStripMenuItem.Text = "&Window";
            this.windowToolStripMenuItem.Click += new System.EventHandler(this.windowToolStripMenuItem_Click);
            // 
            // registersToolStripMenuItem
            // 
            this.registersToolStripMenuItem.Name = "registersToolStripMenuItem";
            this.registersToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.registersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.registersToolStripMenuItem.Text = "&Registers";
            this.registersToolStripMenuItem.Click += new System.EventHandler(this.registersToolStripMenuItem_Click);
            // 
            // memoryToolStripMenuItem
            // 
            this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
            this.memoryToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.memoryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.memoryToolStripMenuItem.Text = "&Memory";
            this.memoryToolStripMenuItem.Click += new System.EventHandler(this.memoryToolStripMenuItem_Click);
            // 
            // assemblyToolStripMenuItem
            // 
            this.assemblyToolStripMenuItem.Name = "assemblyToolStripMenuItem";
            this.assemblyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.assemblyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.assemblyToolStripMenuItem.Text = "&Assembly";
            this.assemblyToolStripMenuItem.Click += new System.EventHandler(this.assemblyToolStripMenuItem_Click);
            // 
            // stackToolStripMenuItem
            // 
            this.stackToolStripMenuItem.Name = "stackToolStripMenuItem";
            this.stackToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.stackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stackToolStripMenuItem.Text = "&Stack";
            this.stackToolStripMenuItem.Click += new System.EventHandler(this.stackToolStripMenuItem_Click);
            // 
            // instantToolStripMenuItem
            // 
            this.instantToolStripMenuItem.Name = "instantToolStripMenuItem";
            this.instantToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.instantToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.instantToolStripMenuItem.Text = "&Instant";
            this.instantToolStripMenuItem.Click += new System.EventHandler(this.instantToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.pauseToolStripMenuItem.Text = "&Pause/Resume";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(755, 774);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Y86 Simulator";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofdOpen;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stepToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid ppgWatcher;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assemblyToolStripMenuItem;
        private System.Windows.Forms.Label lblIcodeD;
        private System.Windows.Forms.Label lblIcodeE;
        private System.Windows.Forms.Label lblIcodeM;
        private System.Windows.Forms.Label lblIcodeW;
        private System.Windows.Forms.Label lblIcodeF;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stackToolStripMenuItem;
        private System.Windows.Forms.Label lblPredPC;
        private System.Windows.Forms.Label lblEsrcB;
        private System.Windows.Forms.Label lblMdstM;
        private System.Windows.Forms.Label lblEsrcA;
        private System.Windows.Forms.Label lblMdstE;
        private System.Windows.Forms.Label lblEdstM;
        private System.Windows.Forms.Label lblEdstE;
        private System.Windows.Forms.Label lblEvalB;
        private System.Windows.Forms.Label lblMvalA;
        private System.Windows.Forms.Label lblWdstM;
        private System.Windows.Forms.Label lblWdstE;
        private System.Windows.Forms.Label lblEvalA;
        private System.Windows.Forms.Label lblMvalE;
        private System.Windows.Forms.Label lblDvalP;
        private System.Windows.Forms.Label lblDvalC;
        private System.Windows.Forms.Label lblDrB;
        private System.Windows.Forms.Label lblDrA;
        private System.Windows.Forms.Label lblCC;
        private System.Windows.Forms.Label lblEvalC;
        private System.Windows.Forms.Label lblMBCH;
        private System.Windows.Forms.Label lblWvalM;
        private System.Windows.Forms.Label lblWvalE;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem instantToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
    }
}

