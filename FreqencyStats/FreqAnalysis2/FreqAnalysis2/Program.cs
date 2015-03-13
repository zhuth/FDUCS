using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FreqAnalysis2
{
    class Program
    {
        static void Main(string[] args)
        {
            IDistanceCalc cfe = new WordTagFreq();
            IRecordReader scr = new SCReader();
            Classifier cls = new Classifier(cfe, scr);

            cls.LoadRecords(@"D:\temp\Ci1.txt", null, 1);
            cls.LoadRecords(@"D:\temp\Ci2.txt", null, 2);
            //cls.CreateDist();
            //cls.DumpDist("cdist.dat");
            cls.LoadDist("cdist.dat");
            
            int[][] res = cls.Classify(0.0002);

            StreamWriter sw = new StreamWriter(@"D:\temp\output.txt");
            foreach (int[] a in res)
            {
                for (int i = 0; i < a.Length; ++i)
                    sw.Write("{0}", (int)cls[a[i]].sourceId);
                sw.WriteLine();
            }

            sw.Close();

            //Console.ReadLine();
        }
    }

    struct Record
    {
        public string text, title;
        public double sourceId;
    }

    class Classifier
    {
        private IDistanceCalc DC;
        private List<Record> recs = new List<Record>();
        private IRecordReader RR;
        private int[] ufs;
        private int[] count;
        private double[,] dist;
        private double[][] central;
        private double _threshold = 0.01;

        public double Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }

        public Record this[int index] {
            get {return recs[index];}
        }


        public Classifier(IDistanceCalc dc, IRecordReader rr)
        {
            this.DC = dc;
            this.RR = rr;
        }

        public void LoadRecords(string filename, Encoding enc = null, double sid = 0)
        {
            if (enc == null) enc = Encoding.GetEncoding("GBK");
            recs.AddRange(RR.ReadRecords(filename, enc, sid));
        }

        public int[][] Classify(double threshold = -1)
        {
            if (threshold < 0) threshold = _threshold;
            
            if (dist == null) CreateDist();

            bool changed = false;
            do
            {
                changed = false;
                for (int i = 0; i < recs.Count - 1; ++i)
                {
                    if (central[i] == null) continue;
                    for (int j = i + 1; j < recs.Count; ++j)
                        if (central[j] != null && ufs[j] != ufs[i] && dist[i,j] < threshold)
                        {
                            ufs[j] = i;
                            compressUfs(j);
                            central[ufs[j]] = adjustCentrals(central[ufs[j]], count[ufs[j]], central[j], count[j]);
                            central[j] = null;
                            count[ufs[j]] += count[j];
                            updateDist(ufs[j]);
                            changed = true;
                        }
                }
            } while (changed);

            List<int[]> lst  = new List<int[]>();
            for (int i = 0; i < ufs.Length; ++i)
            {
                if (ufs[i] < 0) continue;
                lst.Add(getUfs(ufs[i], i));
            }

            return lst.ToArray();
        }

        public void CreateDist()
        {
            ufs = new int[recs.Count]; count = new int[recs.Count];
            central = new double[recs.Count][];
            for (int i = 0; i < recs.Count; ++i)
            {
                ufs[i] = i; count[i] = 1;
                central[i] = DC.GetVector(recs[i].text);
            }

            dist = new double[recs.Count, recs.Count];
            for (int i = 0; i < recs.Count - 1; ++i)
                for (int j = i + 1; j < recs.Count; ++j)
                    dist[i, j] = DC.GetDistance(central[i], central[j]);
            for (int i = 1; i < recs.Count; ++i)
                for (int j = 0; j < i; ++j)
                    dist[j, i] = dist[i, j];
        }

        public void LoadDist(string filename)
        {
            ufs = new int[recs.Count]; count = new int[recs.Count];
            central = new double[recs.Count][];
            for (int i = 0; i < recs.Count; ++i)
            {
                ufs[i] = i; count[i] = 1;
                central[i] = DC.GetVector(recs[i].text);
            }
            
            BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));

            int n = br.ReadInt32(), m = br.ReadInt32();
            dist = new double[n+1, m+1];

            for (int i = 0; i <= n; ++i)
                for (int j = 0; j <= m; ++j)
                    dist[i, j] = br.ReadDouble();

            br.Close();
        }

        public void DumpDist(string filename)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create));

            int n = dist.GetUpperBound(0), m = dist.GetUpperBound(1);

            bw.Write(n); bw.Write(m);

            for (int i = 0; i <= n; ++i)
                for (int j = 0; j <= m; ++j)
                    bw.Write(dist[i, j]);

            bw.Close();
        }

        private void updateDist(int j)
        {
            for (int i = 0; i <= dist.GetUpperBound(0); ++i)
                dist[j, i] = dist[i, j] = DC.GetDistance(central[i], central[j]);
        }

        private double[] adjustCentrals(double[] p, int pc, double[] q, int qc)
        {
            for (int i = 0; i < p.Length; ++i)
                p[i] = (p[i] * pc + q[i] * qc) / (pc + qc);

            return p;
        }

        private int[] getUfs(int c, int offset = 0)
        {
            List<int> lst = new List<int>();
            for (int i = offset; i < ufs.Length; ++i)
                if (ufs[i] == c) { lst.Add(i); ufs[i] = -1; }

            return lst.ToArray();
        }

        private int compressUfs(int j)
        {
            int t = j;
            while (ufs[t] != t)
                t = ufs[t];
            return ufs[j] = t;
        }

    }
}
