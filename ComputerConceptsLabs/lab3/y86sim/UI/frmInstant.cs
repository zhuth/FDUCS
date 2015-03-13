using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace y86sim.UI
{
    public partial class frmInstant : Form
    {
        public frmInstant()
        {
            InitializeComponent();
        }

        private void frmInstant_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        private void frmInstant_Load(object sender, EventArgs e)
        {

        }

        private void txtCmd_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtCmd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            try
            {
                Assembler asm = new Assembler(new string[] { txtCmd.Text });
                foreach (Instruction i in asm.GetInstructions())
                {
                    Program.sim.Processor.F.DoFetch(i);
                    Program.sim.Processor.F.PredPC = Program.sim.Processor.F.Instruction.Address;
                }
                txtHistory.Text += txtCmd.Text + Environment.NewLine;
                txtHistory.SelectionStart = txtHistory.Text.Length - 1;
                txtHistory.ScrollToCaret();
            }
            catch
            {
            }
            finally
            {
            }
            txtCmd.Text = "";
        }

        private void txtCmd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
