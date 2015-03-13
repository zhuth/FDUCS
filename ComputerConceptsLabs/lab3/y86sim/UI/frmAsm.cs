using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace y86sim.UI
{
    public partial class frmAsm : Form
    {
        
        public frmAsm()
        {
            InitializeComponent();
        }

        public new void Show()
        {
            txtArray.ShowSpaces = txtArray.ShowTabs = txtArray.ShowEOLMarkers = false;
            txtArray.IsReadOnly = true;
            StringBuilder sb = new StringBuilder();
            foreach (Instruction i in new InstructionList(Program.sim.Processor.MemoryBlock, Properties.Settings.Default.ProgramLoadMem))
            {
                sb.Append("0x" + i.Address.ToString("x8") + ": " + i.ToString() + Environment.NewLine);
            }
            txtArray.Text = sb.ToString();

            base.Show();
        }

        private void frmAsm_Load(object sender, EventArgs e)
        {
        }

        private void txtArray_DoubleClick(object sender, EventArgs e)
        {
        }

        private void txtArray_Load(object sender, EventArgs e)
        {

        }
    }
}
