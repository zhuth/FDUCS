using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FreqAnalysis2
{
    class LsmCFreqDist : IDistanceCalc
    {
        private string _validClasses = "abcdefghijklmnopqrstuvwxyz";

        public string ValidClasses
        {
            get { return _validClasses; }
            set { _validClasses = value; }
        }
        
        public double[] GetVector(string p)
        {
            double[] tmp = new double[_validClasses.Length];

            Regex r = new Regex(@"/[" + _validClasses + "]+");
            MatchCollection mc = r.Matches(p);
            foreach (Match m in mc)
                if (m.Value.Length > 1) tmp[_validClasses.IndexOf(m.Value[1])] += 1.0 / mc.Count;

            return tmp;
        }

        public double GetDistance(string a, string b)
        {
            if (a.IndexOf('/') < 0 || b.IndexOf('/') < 0)
                return double.PositiveInfinity;

            Dictionary<string, double> da = new Dictionary<string, double>(),
                db = new Dictionary<string, double>();

            Regex r = new Regex(@"/[" + _validClasses + "]");
            MatchCollection mc =r.Matches(a);

            double incr = 1.0 / mc.Count;
            foreach (Match m in mc)
                if (da.ContainsKey(m.Value)) da[m.Value] += incr;
                else da.Add(m.Value, incr);

            mc = r.Matches(b);
            incr = 1.0 / mc.Count;
            foreach (Match m in mc)
                if (db.ContainsKey(m.Value)) db[m.Value] += incr;
                else db.Add(m.Value, incr);

            double sum = 0;
            int kc = da.Keys.Count;

            foreach (string k in da.Keys)
            {
                if (db.ContainsKey(k))
                {
                    sum += Math.Abs(da[k] - db[k]) / (da[k] + db[k]);
                    db.Remove(k);
                }
                else
                    sum += 1;
            }

            foreach (string k in db.Keys) { sum += 1; kc++; }

            return 1 - sum / kc;
        }

        public double GetDistance(double[] a, double[] b)
        {
            double sum = 0;
            int len = Math.Min(a.Length, b.Length);
            for (int i = 0; i < len; ++i)
            {
                sum += a[i] == b[i] ? Math.Abs(a[i] - b[i]) / (a[i] + b[i]) : 0;
                if (a[i] == b[i] && a[i] == 0) len--;
            }

            return 1 - sum / len;
        }
    }
}
