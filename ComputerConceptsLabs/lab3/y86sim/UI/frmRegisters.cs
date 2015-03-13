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
    public partial class frmRegisters : Form
    {
        public frmRegisters()
        {
            InitializeComponent();

            Program.sim.OnClockCycle += new Action(sim_OnClockCycle);
        }

        void sim_OnClockCycle()
        {
            if (this.Visible)
            this.Invoke(new Action(() =>
            {
                ppgWatcher.SelectedObject = Program.sim.Processor.RegisterFile;
            }));
        }

        private void Registers_Load(object sender, EventArgs e)
        {
        }

        private void frmRegisters_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        private void ppgWatcher_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Program.sim.Processor.RegisterFile = (RegisterFile)ppgWatcher.SelectedObject;   
        }
    }
}
