using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace qlearning
{
    public partial class Form1 : Form
    {
        QLearning ql = new QLearning(27);
        int curr = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buildDataGrid();
            picHanoi.Image = new Bitmap(picHanoi.Width, picHanoi.Height);
        }

        private void buildDataGrid()
        {
            for (int i = 0; i < ql.States; ++i)
            {
                dgv.Columns.Add("s" + i, "" + i);
                dgv.Columns[i].Width = 50;
            }
            dgv.Rows.Add(ql.States);
            for( int i=0;i<ql.States; ++i)
                dgv.Rows[i].HeaderCell.Value = "" + i;
            updateDataGrid(ql.Q);
        }

        private void updateDataGrid(double[,] ar)
        {
            for (int i = 0; i <= ar.GetUpperBound(0); ++i)
                for (int j = 0; j <= ar.GetUpperBound(1); ++j)
                {
                    dgv[j, i].Value = ar[j, i].ToString("0.00");
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            curr = 0;
            ql.Train();
            updateDataGrid(ql.Q);
        }

        private bool isHanoiConnected(int i, int j)
        {
            if (i == j) return true;
            int[] pin0 = new int[3], pin1 = new int[3];
            pin0[i % 3] |= 1; pin0[(i / 3) % 3] |= 2; pin0[i / 9] |= 4;
            pin1[j % 3] |= 1; pin1[(j / 3) % 3] |= 2; pin1[j / 9] |= 4;

            int changes = 0, c1 = -1, c2 = 0;
            for (int p = 0; p < 3; ++p)
            {
                changes |= pin1[p] ^ pin0[p];
                if (pin1[p] != pin0[p])
                    if (c1 < 0) c1 = p;
                    else c2 = p;
            }
            if (bitOnes(changes) >= 2) return false;
            if (lowestOne(pin0[c1]) > lowestOne(pin0[c2]))
            {
                int t = c1; c1 = c2; c2 = c1;
            }
            if (lowestOne(pin0[c1]) == (pin0[c2] ^ pin1[c2]))
                return true;
            return false;
        }

        private int bitOnes(int bits)
        {
            int count = 0;
            while (bits > 0) 
            {
                count += bits % 2;
                bits >>= 1;
            }
            return count;
        }

        private int lowestOne(int bits)
        {
            if (bits == 0) return 1 << 16;
            int bit = 1;
            while ((bits & bit) == 0)
                bit <<= 1;
            return bit;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // init Hanoi R
            for (int i = 0; i < ql.States; ++i)
                for (int j = 0; j < ql.States; ++j)
                    if (isHanoiConnected(i, j))
                        if (j == ql.States - 1) ql.R[i, j] = 100;
                        else ql.R[i, j] = 0;
                    else
                        ql.R[i, j] = -1;
            updateDataGrid(ql.R);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drawTower(curr);
            if (curr == ql.States - 1) MessageBox.Show("Done!");
            curr = ql.GetNextState(curr);
        }

        private void drawTower(int state)
        {
            Graphics g = Graphics.FromImage(picHanoi.Image);
            g.Clear(Color.White);
            bool[,] pin = new bool[3, 3];
            pin[state % 3, 0] = true; pin[(state / 3) % 3, 1] = true; pin[state / 9, 2] = true;
            for (int i = 0; i < 3; ++i)
            {
                // draw tower i
                int y = picHanoi.Image.Height - 20;
                int x = (picHanoi.Image.Width - 70) / 3 * (i + 1);
                for(int j = 2; j >= 0; --j)
                    if (pin[i, j])
                    {
                        int w = (j+1)*20, h = (j+1)*10;
                        g.FillEllipse(new SolidBrush(Color.Red), x - w / 2, y - h, w, h);
                        y -= h;
                    }
            }
            picHanoi.Refresh();
        }
    }
}
