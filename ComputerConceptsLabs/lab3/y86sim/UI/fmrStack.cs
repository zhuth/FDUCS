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
    public partial class frmStack : Form
    {
        private int esp0 = 0, ebp0 = 0;


        public frmStack()
        {
            InitializeComponent();
            Program.sim.OnClockCycle += new Action(sim_OnClockCycle);
        }

        public void sim_OnClockCycle()
        {
            if (this.Visible) this.Invoke(new Action(() =>
               {

                   int esp = Program.sim.Processor.RegisterFile.ESP;
                   int ebp = Program.sim.Processor.RegisterFile.EBP;
                   if (esp != esp0 || ebp != ebp0)
                   {
                       lstStack.Items.Clear();
                       for (int i = Properties.Settings.Default.StackLoadMem; i > esp; i -= 4)
                       {
                           lstStack.Items.Add(Program.sim.Processor.MemoryBlock.GetInt(i - 4));
                           if (i == ebp) lstStack.SelectedIndex = lstStack.Items.Count - 1;
                       }
                       esp0 = esp; ebp0 = ebp;
                   }

                   this.Text = string.Format("Stack [ESP = {0}, EBP = {1}]", Program.sim.Processor.RegisterFile.ESP, Program.sim.Processor.RegisterFile.EBP);
               }));            
        }

        private void fmrStack_Load(object sender, EventArgs e)
        {

        }

        private void frmStack_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }
    }
}
