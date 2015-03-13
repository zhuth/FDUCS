using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreqStatMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3) return;
            string output = args[args.Length - 1];
            Dictionary<string, int>[] counter = new Dictionary<string, int>[args.Length - 1];
            Dictionary<string, int> gcounter = new Dictionary<string, int>();
            for (int i = 0; i < args.Length - 1; ++i)
            {
                counter[i] = new Dictionary<string, int>();
                foreach (string s in System.IO.File.ReadAllLines(args[i]))
                {
                    string[] sp = s.Split('\t');
                    if (sp.Length < 2) continue;
                    int c = 0;
                    if (!int.TryParse(sp[1], out c)) continue;
                    counter[i].Add(sp[0], c);
                    if (!gcounter.ContainsKey(sp[0]))
                        gcounter.Add(sp[0], c);
                    else
                        gcounter[sp[0]] += c;
                }
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(output))
            {
                foreach (string k in gcounter.Keys)
                {
                    sw.Write(k);
                    for (int i = 0; i < args.Length - 1; ++i)
                    {
                        int v = 0;
                        if (counter[i].ContainsKey(k)) v = counter[i][k];
                        sw.Write("\t" + v);
                    }
                    sw.WriteLine("\t" + gcounter[k]);
                }
            }
        }
    }
}
