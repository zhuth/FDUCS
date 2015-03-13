using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace svdc
{
    public class BitmapOperations
    {
        public static void ReadBitmapFile(string filename, out double[,] r, out double[,] g, out double[,] b,
            out int w, out int h)
        {
            Bitmap bmp =(Bitmap)Image.FromFile(filename);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            r = new double[bd.Height, bd.Width];
            g = new double[bd.Height, bd.Width];
            b = new double[bd.Height, bd.Width];

            w = bd.Width; h = bd.Height;

            unsafe
            {
                int stride = bd.Stride - bd.Width * 3;
                byte* pin = (byte*)(bd.Scan0.ToPointer());

                for (int y = 0; y < bd.Height; ++y)
                {
                    for (int x = 0; x < bd.Width; ++x)
                    {
                        r[y, x] = pin[2];
                        g[y, x] = pin[1];
                        b[y, x] = pin[0];
                        pin += 3;
                    }
                    pin += stride;
                }
            }

            bmp.UnlockBits(bd);
        }

        public static void WriteBitmapFile(string filename, double[,] r, double[,] g, double[,] b)
        {
            throw new NotImplementedException();
        }

        public static Bitmap DrawBitmap(int w, int h, double[,] r, double[,] g, double[,] b)
        {
            Bitmap bmp = new Bitmap(w, h);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                int stride = bd.Stride - bd.Width * 3;
                byte* pout = (byte*)(bd.Scan0.ToPointer());

                for (int y = 0; y < bd.Height; ++y)
                {
                    for (int x = 0; x < bd.Width; ++x)
                    {
                        pout[2] = fixByte(r[y, x]);
                        pout[1] = fixByte(g[y, x]);
                        pout[0] = fixByte(b[y, x]);
                        pout += 3;
                    }
                    pout += stride;
                }
            }

            bmp.UnlockBits(bd);
            return bmp;
        }

        private static byte fixByte(double x)
        {
            if (x > 255) return (byte)255;
            if (x < 0) return 0;
            return (byte)x;
        }
    }
}