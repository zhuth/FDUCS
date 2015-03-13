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
    public partial class frmArrayList : Form
    {
        private int index = 0;

        public frmArrayList()
        {
            InitializeComponent();
        }

        internal void Show(byte[] ary)
        {
            var p = txtArray.ActiveTextAreaControl.Caret.Position;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ary.Length; ++i)
            {
                sb.Append(ary[i].ToString("x2") + " ");
                if ((i + 1) % 100 == 0)
                {
                    sb.Append(Environment.NewLine);
                }
            }

            txtArray.ShowSpaces = txtArray.ShowTabs = txtArray.ShowEOLMarkers = false;
            txtArray.IsReadOnly = true;
            txtArray.Text = sb.ToString();
            txtArray.ActiveTextAreaControl.Caret.Position = p;
        }

        private void changeCaret()
        {
            while (this.Visible)
            {
                int line = txtArray.ActiveTextAreaControl.Caret.Line;
                int col = txtArray.ActiveTextAreaControl.Caret.Column / 3;
                this.Invoke(new Action(() => { this.Text = "Index: " + (index = line * 100 + col); }));

                Thread.Sleep(100);
            }
        }

        private void frmArrayList_Load(object sender, EventArgs e)
        {
            
        }

        private void txtArray_DoubleClick(object sender, EventArgs e)
        {
        }

        private void txtArray_Load(object sender, EventArgs e)
        {

        }


        public void UpdateContent(byte[] array)
        {
            Show(array);
        }

        private void frmArrayList_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        private void frmArrayList_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Thread thrCC = new Thread(new ThreadStart(changeCaret));
                thrCC.Start();
            }
        }
    }
}
