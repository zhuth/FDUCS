using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace y86sim
{
    public partial class frmMain : Form
    {
        private const string LOADING = "Loading...";
        private const int ARRAYMAX = 50;
        private UI.frmArrayList fal = new UI.frmArrayList();
        private UI.frmRegisters freg = new UI.frmRegisters();
        private UI.frmStack fstk = new UI.frmStack();
        private UI.frmInstant fist = new UI.frmInstant();

        public frmMain()
        {
            InitializeComponent();

            Program.sim.OnClockCycle += new Action(sim_OnClockCycle);
            Program.sim.OnSimulatorHalt += new ProcessorHaltEventHandler(sim_OnSimulatorHalt);
            ppgWatcher.SelectedObject = Program.sim.Processor;
        }

        private void sim_OnSimulatorHalt(int eax)
        {
            MessageBox.Show("Program halt. \r\nEAX = " + eax);
        }

        private void sim_OnClockCycle()
        {
            this.Invoke(new Action(() =>
            {
                // updateWatchTree(Program.sim, tvWatcher.Nodes[0]);
                ppgWatcher.SelectedObject = Program.sim.Processor;

                fal.UpdateContent(Program.sim.Processor.MemoryBlock.Buffer);

                // update labels
                lblIcodeD.Text = Program.sim.Processor.D.Instruction.ToString();
                lblIcodeM.Text = Program.sim.Processor.M.Instruction.ToString();
                lblIcodeW.Text = Program.sim.Processor.W.Instruction.ToString();
                lblIcodeF.Text = Program.sim.Processor.F.Instruction.ToString();
                lblIcodeE.Text = Program.sim.Processor.E.Instruction.ToString();

                lblIcodeD.ForeColor = getColor(Program.sim.Processor.D);
                lblIcodeM.ForeColor = getColor(Program.sim.Processor.M);
                lblIcodeW.ForeColor = getColor(Program.sim.Processor.W);
                lblIcodeF.ForeColor = getColor(Program.sim.Processor.F);
                lblIcodeE.ForeColor = getColor(Program.sim.Processor.E);

                lblPredPC.Text = Program.sim.Processor.F.PredPC.ToString();

                lblDrA.Text = Program.sim.Processor.D.Ra.ToString();
                lblDrB.Text = Program.sim.Processor.D.Rb.ToString();
                lblDvalC.Text = Program.sim.Processor.D.ValC.ToString();
                lblDvalP.Text = Program.sim.Processor.D.ValP.ToString();

                lblEdstE.Text = Program.sim.Processor.E.DstE.ToString();
                lblEdstM.Text = Program.sim.Processor.E.DstM.ToString();
                lblEsrcA.Text = Program.sim.Processor.E.SrcA.ToString();
                lblEsrcB.Text = Program.sim.Processor.E.SrcB.ToString();
                lblEvalA.Text = Program.sim.Processor.E.ValA.ToString();
                lblEvalB.Text = Program.sim.Processor.E.ValB.ToString();
                lblEvalC.Text = Program.sim.Processor.E.ValC.ToString();

                lblMBCH.Text = Program.sim.Processor.M.Bch.ToString();
                lblMdstE.Text = Program.sim.Processor.M.DstE.ToString();
                lblMdstM.Text = Program.sim.Processor.M.DstM.ToString();
                lblMvalA.Text = Program.sim.Processor.M.ValA.ToString();
                lblMvalE.Text = Program.sim.Processor.M.ValE.ToString();

                lblWdstE.Text = Program.sim.Processor.W.DstE.ToString();
                lblWdstM.Text = Program.sim.Processor.W.DstM.ToString();
                lblWvalE.Text = Program.sim.Processor.W.ValE.ToString();
                lblWvalM.Text = Program.sim.Processor.W.ValM.ToString();

                lblCC.Text = Program.sim.Processor.E.ConditionCodes.ToString();
            }));
        }

        private Color getColor(Stage stg)
        {
            if (stg.Bubble) return Color.Red;
            if (stg.Stall) return Color.Blue;
            return Color.Black;
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            ppgWatcher.SelectedObject = Program.sim;
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            Program.sim.Step();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdOpen.ShowDialog() != DialogResult.OK) return;
            Program.sim.Initialize(ofdOpen.FileName, Utility.ChangeExtName(ofdOpen.FileName, "txt"), Properties.Settings.Default.ProgramLoadMem, Properties.Settings.Default.StackLoadMem);
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStep_Click(sender, e);
        }

        private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memoryToolStripMenuItem.Checked = fal.Visible = !fal.Visible;
        }

        private void registersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registersToolStripMenuItem.Checked = freg.Visible = !fal.Visible;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.frmSettings fset = new UI.frmSettings();
            fset.ShowDialog();
        }

        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registersToolStripMenuItem.Checked = freg.Visible;
            stackToolStripMenuItem.Checked = fstk.Visible;
            memoryToolStripMenuItem.Checked = fal.Visible;
            instantToolStripMenuItem.Checked = fist.Visible;
        }

        private void assemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.frmAsm fasm = new UI.frmAsm();
            fasm.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Application.ExecutablePath, " -help -pause");
        }

        private void stackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stackToolStripMenuItem.Checked = fstk.Visible = !fstk.Visible;
        }

        private void instantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instantToolStripMenuItem.Checked = fist.Visible = !fist.Visible;
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (runToolStripMenuItem1.Text == "&Run")
            {
                Program.sim.Run();
                runToolStripMenuItem1.Text = "&Stop";
            } else {
                Program.sim.Stop();
                runToolStripMenuItem1.Text = "&Run";
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.sim.Processor.Clock.Enabled)
                Program.sim.Processor.Clock.Stop();
            else
                Program.sim.Processor.Clock.Start();
        }
    }
}
