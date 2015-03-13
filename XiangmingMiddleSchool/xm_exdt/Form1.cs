using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace exdt
{
    public partial class Form1 : Form
    {
        SerialPort sp = new SerialPort();
        int step = 0;
        List<int> dists = new List<int>(10);
        Graphics g;

        public Form1()
        {
            InitializeComponent();
            sp.ReadTimeout = 10000;
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[4];
            int len = sp.Read(data, 0, 4);
            int dist = ((data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3]);
            Console.WriteLine("" + dist);

            if (dist == -128)
            {
                dists.Clear();
            }
            else if (dist == -127)
            {
                this.Invoke(new Action(drawDots));
            }
            else
            {
                dists.Add(dist);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
            sp.PortName = cbxSP.Text;
            sp.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            step = 0; g.Clear(Color.White); 
            g.DrawLine(Pens.Black, new Point(0, pictureBox1.Height / 2), new Point(pictureBox1.Width, pictureBox1.Height / 2));
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            lblPos.Text = string.Format("({0}, {1})", e.X, pictureBox1.Height / 2 - e.Y);
        }

        private void drawDots()
        {
            if (dists.Count == 0) return;
            Pen pen = new Pen(Brushes.Red, 3);
            int count = dists.Count;
            double deg = Math.PI / count, deg0 = deg / 2;
            int height = pictureBox1.Height;
            for (int i = 0; i < count; ++i)
            {
                float x = (float)(step + Math.Sin(deg * i + deg0) * dists[i] * 5),
                    y = (float)(height / 2 - dists[i] * 5 * Math.Cos(deg * i + deg0));
                g.DrawEllipse(pen, x, y, 3, 3);
                // g.DrawString("" + i, Font, Brushes.Green, x, y);
            }
            pictureBox1.Refresh();
            step += int.Parse(textBox1.Text);
        }
    }
}
