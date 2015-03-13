using System;
using System.Collections.Generic;
using System.Text;

namespace svdc
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            double eps = 1e-2;

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: svdc <input bitmap> [threshold]");
                return;
            }

            if (args.Length >= 2)
                eps = double.Parse(args[2]);

            double[][,] clr = new double[3][,], opt = new double[3][,];
            int w, h;
            BitmapOperations.ReadBitmapFile(args[0], out clr[0], out clr[1], out clr[2], out w, out h);

            double[][,] u = new double[3][,], vt = new double[3][,];
            double[][] s = new double[3][];
            Console.Write("Threshold = {0}. ", eps);

            Console.Write("Processing");
            for (int color = 0; color < 3; ++color)
            {
                Console.Write(".");
                opt[color] = new double[h, w];
                alglib.rmatrixsvd(clr[color], h, w, 2, 2, 2, out s[color], out u[color], out vt[color]);
                for (int lo = 0; s[color][lo] > eps * s[color][0]; ++lo)
                {
                    for (int i = 0; i < h; ++i)
                        for (int j = 0; j < w; ++j)
                            opt[color][i, j] += u[color][i, lo] * vt[color][lo, j] * s[color][lo];
                }
            }

            // output
            System.Windows.Forms.Form g = new System.Windows.Forms.Form();
            g.Width = w + 100; g.Height = h + 100;
            g.Text = "input";
            g.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            g.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            g.BackgroundImage = System.Drawing.Image.FromFile(args[0]);
            g.Show();

            System.Windows.Forms.Form f = new System.Windows.Forms.Form();
            f.Width = w + 100; f.Height = h + 100;
            f.Text = "output";
            f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            f.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;

            System.Drawing.Bitmap bmp = BitmapOperations.DrawBitmap(w, h, opt[0], opt[1], opt[2]);
            bmp.Save("output.bmp");
            f.BackgroundImage = bmp;

            f.ShowDialog();
            Console.WriteLine("\r\nClose the 'output' window to exit.");
        }
    }
}
