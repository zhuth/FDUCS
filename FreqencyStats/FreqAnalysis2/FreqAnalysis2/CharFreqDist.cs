using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreqAnalysis2
{
    class CharFreqEularDist : IDistanceCalc
    {
        const int ChineseCharRangeU = 0x9FFF;
        const int ChineseCharRangeL = 0x4E00;

        public double[] GetVector(string p)
        {
            double[] tmp = new double[ChineseCharRangeU - ChineseCharRangeL + 1];

            int c = 0;
            for (int i = 0; i < p.Length; ++i)
            {
                if ((int)p[i] >= ChineseCharRangeL && (int)p[i] <= ChineseCharRangeU)
                {
                    c++;
                    tmp[(int)p[i] - ChineseCharRangeL] += 1.0;
                }
            }

            for (int i = 0; i < tmp.Length; ++i) tmp[i] /= c;

            return tmp;
        }

        public double GetDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0;

            Dictionary<string, double> d = new Dictionary<string, double>();

            double incr = 1.0 / a.Length;
            for (int i = 0; i < a.Length; ++i)
                if (d.ContainsKey("" + a[i])) d["" + a[i]] += incr;
                else d.Add("" + a[i], incr);

            incr = 1.0 / b.Length;

            for (int i = 0; i < b.Length; ++i)
                if (d.ContainsKey("" + b[i])) d["" + b[i]] -= incr;
                else if (d.ContainsKey("b" + b[i])) d["b" + b[i]] += incr;
                else d.Add("b" + b[i], incr);

            double sum =0;
            foreach (string k in d.Keys)
                sum += Math.Pow(d[k], 2);

            return sum;
        }

        public double GetDistance(double[] a, double[] b)
        {
            if (a == null || b == null) return 0;
            double sum = 0;
            int len = Math.Min(a.Length, b.Length);
            for (int i = 0; i < len; ++i)
                sum += Math.Pow(a[i] - b[i], 2);

            return sum;
        }
    }
}
