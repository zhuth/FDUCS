using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreqAnalysis2
{
    class WordTagFreq : IDistanceCalc
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        Dictionary<string, int> dictid = new Dictionary<string, int>();
        int minWLen = 100, maxWLen = 0;

        public WordTagFreq()
        {
            loadDict("words.lst");
        }

        public WordTagFreq(string dictFile) { loadDict(dictFile); }

        private void loadDict(string dictFile) {
            foreach (string x in File.ReadAllLines(dictFile, Encoding.GetEncoding("GBK")))
            {
                string[] token = x.Split('\t');
                if (token.Length > 1)
                {
                    if (!dict.ContainsKey(token[0]))
                    {
                        dict.Add(token[0], token[1]);
                        dictid.Add(token[0], dict.Count);
                        minWLen = Math.Min(minWLen, token[0].Length);
                        maxWLen = Math.Max(maxWLen, token[0].Length);
                    }
                }
            }
        }

        public double GetDistance(string a, string b)
        {
            return GetDistance(GetVector(a), GetVector(b));
        }

        public double GetDistance(double[] a, double[] b)
        {
            return (new CharFreqEularDist()).GetDistance(a, b);
        }

        public double[] GetVector(string p)
        {
            double[] tmp = new double[dict.Count];
            int wc = 0;
            for (int i = 0; i < p.Length-minWLen; ++i)
            {
                for (int j = minWLen; j <= maxWLen && (i+j)<p.Length; ++j)
                    if (dict.ContainsKey(p.Substring(i, j)))
                    {
                        tmp[dictid[p.Substring(i, j)]]++;
                        i += j;
                        break;
                    }
                wc++;
            }
            for (int i = 0; i < tmp.Length; ++i) tmp[i] /= wc;
            return tmp;
        }
    }
}
