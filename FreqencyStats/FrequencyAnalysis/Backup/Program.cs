using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FrequencyAnalysis
{
    class Program
    {
        static Dictionary<string, int> dict = new Dictionary<string, int>();
        static Dictionary<string, int> dictattr = new Dictionary<string, int>();
        static Dictionary<string, int> dictspec = new Dictionary<string, int>();

        static string[] lines;
        static string lastWhere = "", lastCmd = "", sensitive = "", inFile = "";
        static int ptr = -1;
        static Regex regQuery;


        static void Main(string[] args)
        {

            if (args.Length < 2) { Console.WriteLine("请指定分词结果表和关键词。"); return; }
            string outFile = args[0].Replace(".ana.txt", "") + ".freq.txt";
            
            //Console.WriteLine("Press <Enter> for interactive query mode, G for generating statistics file, and X for quitting.");
            //ConsoleKey select = Console.ReadKey().Key;
            inFile = args[0];
            sensitive = args[1];

            lines = File.ReadAllLines(args[0]);
            while (true)
            {
                Console.Write("Query >");
                string cmd = Console.ReadLine().Trim();
                if (cmd == "exit") break;
                deal(cmd);
            }

        }

        private static void generateFile(string inFile, string outFile, string sensitive)
        {

            using (StreamWriter sw = new StreamWriter(outFile))
            {
                //HashSet<string> hs = new HashSet<string>();
                StreamReader sr = new StreamReader(inFile);
                //int count = 0;
                while (!sr.EndOfStream)
                {
                    string[] s = sr.ReadLine().Split(' ');
                    string prefix = "", suffix = "";
                    int id = 0;

                    while (id < s.Length)
                    {
                        for (id++; id < s.Length && s[id].IndexOf(sensitive) < 0; ++id) ;
                        if (id >= s.Length || s[id].IndexOf(sensitive) < 0) break;
                        if (id > 1) { prefix = s[id - 1]; } else { prefix = ""; }
                        if (id < s.Length - 1) { suffix = s[id + 1]; } else { suffix = ""; }

                        string attrpre = "", attrsuf = "";
                        attrpre = prefix.IndexOf('/') > 0 ? prefix.Substring(prefix.IndexOf('/') + 1) : "";
                        attrsuf = suffix.IndexOf('/') > 0 ? suffix.Substring(suffix.IndexOf('/') + 1) : "";
                        prefix = prefix.IndexOf('/') > 0 ? prefix.Substring(0, prefix.IndexOf('/')) : "";
                        suffix = suffix.IndexOf('/') > 0 ? suffix.Substring(0, suffix.IndexOf('/')) : "";

                        string sse = s[id]; if (sse.IndexOf('/') > 0) sse = sse.Substring(0, sse.IndexOf('/'));
                        if (!dictspec.ContainsKey(sse)) dictspec.Add(sse, 0);
                        dictspec[sse]++;

                        if (!s[id].StartsWith(sensitive + "/"))
                        {
                            continue;
                        }

                        if (!dict.ContainsKey(prefix + "," + suffix)) dict.Add(prefix + "," + suffix, 0);
                        dict[prefix + "," + suffix]++;

                        if (!dictattr.ContainsKey(attrpre + "," + attrsuf)) dictattr.Add(attrpre + "," + attrsuf, 0);
                        dictattr[attrpre + "," + attrsuf]++;
                    }

                }

                int td, ta, ts = ta = td = 0;
                foreach (string key in dict.Keys)
                {
                    sw.WriteLine("x\t{0}\t{1}", key, dict[key]);
                    td += dict[key];
                }

                foreach (string key in dictattr.Keys)
                {
                    sw.WriteLine("a\t{0}\t{1}", key, dictattr[key]);
                    ta += dictattr[key];
                }

                foreach (string key in dictspec.Keys)
                {
                    sw.WriteLine("b\t{0}\t{1}", key, dictspec[key]);
                    ts += dictspec[key];
                }

                int dc;
                dc = 0;
                var most50 = dict.OrderByDescending(i => i.Value).Take(50);
                foreach (var v in most50)
                {
                    sw.Write(v.Key + "(" + v.Value + ")\t");
                    dc++; if (dc % 5 == 0) sw.WriteLine();
                }
                Console.WriteLine(td + "," + dict.Count + "\r\n=====================");
                dc = 0;
                most50 = dictattr.OrderByDescending(i => i.Value).Take(50);
                foreach (var v in most50)
                {
                    sw.Write(v.Key + "(" + v.Value + ")\t");
                    dc++; if (dc % 5 == 0) sw.WriteLine();
                }
                sw.WriteLine(ta + "," + dictattr.Count + "\r\n=====================");
                dc = 0;
                most50 = dictspec.OrderByDescending(i => i.Value).Take(50);
                foreach (var v in most50)
                {
                    sw.Write(v.Key + "(" + v.Value + ")\t");
                    dc++; if (dc % 5 == 0) sw.WriteLine();
                }
                sw.WriteLine(ts + "," + dictspec.Count);
                sr.Close();
            }
                    
        }

        private static bool deal(string cmd)
        {

            string token = cmd.Split(' ')[0].ToLower();
            string param = token.Length == cmd.Length ? "" : cmd.Substring(token.Length + 1).Trim();
            if (param.Length > 1 && param[0] == '\"' && param[param.Length - 1] == '\"') param = param.Substring(1, param.Length - 2);
            switch (token)
            {
                case "where":
                    if (param == "" || param == lastWhere) param = lastWhere; else { lastWhere = param; ptr = -1; }
                    for (++ptr; ptr < lines.Length && !lines[ptr].Contains(param); ++ptr) ;
                    if (ptr < lines.Length && lines[ptr].Contains(param))
                        Console.WriteLine("Line #{0}: {1}", ptr, lines[ptr]);
                    else
                    {
                        Console.WriteLine("No more matches for \"{0}\".", param);
                        return false;
                    }
                    break;

                case "wherex":
                    if (param == "" || param == lastWhere) param = lastWhere; else { lastWhere = param; ptr = -1; }
                    try
                    {
                        regQuery = new Regex(param);
                        for (++ptr; ptr < lines.Length && !regQuery.IsMatch(lines[ptr]); ++ptr) ;
                        if (ptr < lines.Length && regQuery.IsMatch(lines[ptr]))
                            Console.WriteLine("Line #{0}: {1}", ptr, lines[ptr]);
                        else
                        {
                            Console.WriteLine("No more matches.");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                    break;

                case "count":
                    if (param == "" || param == lastWhere) param = lastWhere; else { lastWhere = param; ptr = -1; }
                    Console.WriteLine(lines.Where(x => x.Contains(lastWhere)).Count());
                    break;

                case "countx":
                    if (param == "" || param == lastWhere) param = lastWhere; else { lastWhere = param; ptr = -1; }                    
                    try{
                        regQuery = new Regex(param);
                        Console.WriteLine(lines.Where(x => regQuery.IsMatch(x)).Count());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                    break;

                case "repeat":
                    int repeatParam = 0;
                    if (param == "" || !int.TryParse(param, out repeatParam))
                    {
                        Console.WriteLine("Please specify a parameter.");
                        return false;
                    }
                    else
                        while (repeatParam-- > 0 && deal(lastCmd)) ;

                    break;

                case "source":
                    int sourceParam = 0;
                    if (param == "" || !int.TryParse(param, out sourceParam))
                    {
                        Console.WriteLine("Please specify a parameter.");
                        return false;
                    }
                    else{
                        try
                        {
                            using (StreamReader sr = new StreamReader(inFile.Replace(".ana.txt", ".txt")))
                            {
                                while (--sourceParam > 0 && !sr.EndOfStream) sr.ReadLine();
                                Console.WriteLine(sr.ReadLine());

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            return false;
                        }
                    }

                    break;

                case "output":
                    string[] outputParams = takeTokens(param);

                    if (outputParams.Length < 2)
                    {
                        Console.WriteLine("Please specify parameters.");
                        return false;
                    }

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(outputParams[1]))
                        {
                            Dictionary<string, int> outputCount = new Dictionary<string,int>();
                            Regex regReplacer = new Regex(outputParams[0]);
                            foreach (string p in lines)
                            {
                                string tmp = "";
                                foreach (Match m in regReplacer.Matches(p))
                                {
                                    tmp += m.Value;
                                }
                                if (!outputCount.ContainsKey(tmp)) outputCount.Add(tmp, 0);
                                outputCount[tmp]++;

                                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Q) break;
                            }

                            foreach (string key in outputCount.Keys)
                                if (outputCount[key] >= 5)
                                    sw.WriteLine(outputCount[key] + "\t" + key);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }

                    Console.WriteLine("Output succeeded.");

                    break;

                case "generate":
                    generateFile(inFile, inFile.Replace(".txt", "") + ".freq.txt", sensitive);
                    break;
            }


            if (token != "repeat" && token != "generate")
                lastCmd = cmd;

            return true;
        }


        private static string[] takeTokens(string cmd)
        {
            cmd += " ";
            List<string> res = new List<string>();
            bool quote = false;
            string tmp = "";
            for (int i = 0; i < cmd.Length; ++i)
            {

                if (cmd[i] == ' ' && !quote)
                {
                    res.Add(tmp); tmp = "";
                }

                if (cmd[i] == '\"') quote = !quote;
                else tmp += cmd[i];
            }

            return res.ToArray();
        }
    }
}
