/*
 * Created by SharpDevelop.
 * User: st
 * Date: 2012-3-27
 * Time: 10:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace cs.microcommand
{
    class Program
    {
        static int reverse(string bit)
        {
            if (bit == "0") return 1; else return 0;
        }

        static int parseNum(string num)
        {
            num = num.ToUpper();
            if (num.StartsWith("B"))
            {
                num = num.Substring(1);
                try
                {
                    return Convert.ToInt32(num, 2);
                }
                catch { }
            }
            else if (num.StartsWith("H"))
            {
                num = num.Substring(1);
                try
                {
                    return Convert.ToInt32(num, 16);
                }
                catch { }
            }
            else if (num.StartsWith("D"))
            {
                num = num.Substring(1);
                try
                {
                    return Convert.ToInt32(num, 10);
                }
                catch { }
            }
            else
            {
                int r = 0;
                if (int.TryParse(num, out r)) return r;
            }
            return -1;
        }

        static void usage()
        {
            Console.WriteLine("Usage: cs.microcommand.exe <arch config text> [-vhd <vhd signal assigning output>] [-m19 <m19 output>] [-csv <csv output>]\n\nZhu Tianhua 09300240004 zthpublic@gmail.com. Mar 2012");
        }

        public static void Main(string[] argv)
        {
            bool pause = false;
            if (argv.Length < 1)
            {
                usage();
                return;
            }

            string vhdfile = "", csvfile = "", m19file = "", deffile = "";

            try
            {
                for (int i = 0; i < argv.Length; ++i)
                {
                    switch (argv[i])
                    {
						case "-def":
							deffile = argv[i + 1];
                        case "-vhd":
                            vhdfile = argv[i + 1];
                            break;
                        case "-csv":
                            csvfile = argv[i + 1];
                            break;
                        case "-m19":
                            m19file = argv[i + 1];
                            break;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                usage(); return;
            }

            string f = argv[0];
            int[] buffer = new int[1024];
            string[] lines = System.IO.File.ReadAllLines(f);

            Dictionary<string, string> valids = new Dictionary<string, string>();
            Dictionary<string, string> defaults = new Dictionary<string, string>();
            Dictionary<string, string[]> symbols = new Dictionary<string, string[]>();
            Dictionary<string, int> cmds = new Dictionary<string, int>(),
                cmdBeatCount = new Dictionary<string, int>();
            Dictionary<string, int> sigbits = new Dictionary<string, int>(),
                sigrefs = new Dictionary<string,int>();

            HashSet<int> hsCmdNum = new HashSet<int>();

            string section = "";
            string curr_cmd = "";

            int beat = 0, beatcount = 0, maxCommand = 0, valFetch = 0;

            for (int lineno = 0; lineno < lines.Length; ++lineno)
            {
                string s = lines[lineno].Trim();
                if (s == "") continue;
                if (s.StartsWith("#")) continue;
                if (s.StartsWith("-"))
                {
                    section = s.Substring(1).ToLower();
                    if (section == "cmds")
                    {
                        foreach (string sig in defaults.Keys)
                        {
                            sigrefs.Add(sig, 0);
                        }
                    }
                    continue;
                }
                switch (section)
                {
                    case "valid":
                        s = s.Replace(" ", "");
                        string[] eq = s.Split('=');
                        eq[0] = eq[0].Trim();
                        if (valids.ContainsKey(eq[0]))
                        {
                            Console.WriteLine("Line {0}: {1} already exists.", lineno, eq[0]); pause = true;
                            continue;
                        }
                        if (eq.Length < 2)
                        {
                            Console.WriteLine("Line {0}: valid value required.", lineno); pause = true;
                            continue;
                        }
                        if (eq[1].Contains(","))
                        {
                            string[] def = eq[1].Split(',');
                            if (def[0] == "")
                            {
                                Console.WriteLine("Line {0}: invalid value for valid value of {1}.", lineno, eq[0]); pause = true;
                                continue;
                            }
                            valids.Add(eq[0], def[0]);
                            if (def[1] != "")
                                defaults.Add(eq[0], def[1]);
                            else
                                defaults.Add(eq[0], "" + reverse(def[0]));
                        }
                        else
                        {
                            valids.Add(eq[0], eq[1]);
                            defaults.Add(eq[0], "" + reverse(eq[1]));
                        }
                        sigbits.Add(eq[0], sigbits.Count);
                        break;

                    case "symb":
                        string[] symbs = s.Split('=');
                        if (symbs.Length < 2)
                        {
                            Console.WriteLine("Line {0}: Symbol definition misformated.", lineno); pause = true;
                            continue;
                        }
                        if (valids.ContainsKey(symbs[0]))
                        {
                            Console.WriteLine("Line {0}: Symbol name cannot be the same as signal name.", lineno); pause = true;
                            continue;
                        }
                        if (symbols.ContainsKey(symbs[0]))
                        {
                            Console.WriteLine("Line {0}: Symbol name already exists.", lineno); pause = true;
                            continue;
                        }
                        string[] symbRep = symbs[1].Split(' ');
                        foreach (string rep in symbRep)
                        {
                            if (rep == "") continue;
                            if (!valids.ContainsKey(rep))
                            {
                                Console.WriteLine("Line {0}: Unknown signal name {1}.", lineno, rep); pause = true; continue;
                            }
                        }
                        symbols.Add(symbs[0], symbRep);
                        break;

                    case "cmds":
                        if (s.EndsWith(":"))
                        {
                            beatcount = 0;
                            string cmdname = s.Substring(0, s.Length - 1);
                            if (cmdname.ToLower() == "fetch")
                            {
                                cmds.Add("fetch", -128); curr_cmd = "fetch"; cmdBeatCount.Add(curr_cmd, 0); continue;
                            }
                            if (cmds.ContainsKey(cmdname))
                            {
                                Console.WriteLine("Line {0}: Coomand {1} already defined.", lineno, cmdname); pause = true; continue;
                            }
                            curr_cmd = cmdname;
                            continue;
                        }
                        if (!cmds.ContainsKey(curr_cmd))
                        {
                            int cmdnum = parseNum(s);
                            if (cmdnum < 0 || cmdnum >= 32)
                            {
                                Console.WriteLine("Line {0}: Invalid command numbering.", lineno, cmdnum); pause = true;
                                cmds.Add(curr_cmd, -1);
                                continue;
                            }
                            if (hsCmdNum.Contains(cmdnum))
                            {
                                Console.WriteLine("Line {0}: Command numbered {1} already exists.", lineno, cmdnum); pause = true;
                                cmds.Add(curr_cmd, cmdnum);
                                continue;
                            }
                            cmds.Add(curr_cmd, cmdnum);
                            hsCmdNum.Add(cmdnum);
                            cmdBeatCount.Add(curr_cmd, 0);
                            maxCommand = Math.Max(maxCommand, cmdnum);
                            continue;
                        }

                        ++beatcount; beat = beatcount - 1;
                        string[] sigs = s.Split(' ');
                        ++cmdBeatCount[curr_cmd];

                        int val = 0;

                        Dictionary<string, string> vals = new Dictionary<string, string>(defaults);

                        for (int sigstart = 0; sigstart < sigs.Length; ++sigstart)
                        {
                            string sigt = sigs[sigstart];
                            string sign = sigt, sigv = "";
                            if (sigt.Contains("="))
                            {
                                sign = sigt.Split('=')[0];
                                sigv = sigt.Split('=')[1];
                            }
                            else
                            {
                                if (defaults.ContainsKey(sign))
                                {
                                    sigv = "" + reverse(defaults[sign]);
                                    sigrefs[sign]++;
                                }
                            }
                            if (valids.ContainsKey(sign))
                            {
                                vals[sign] = sigv;
                            }
                            else if (symbols.ContainsKey(sign))
                            {
                                for (int i = 0; i < symbols[sign].Length; ++i)
                                {
                                    vals[symbols[sign][i]] = "" + sigv[i];
                                    sigrefs[symbols[sign][i]]++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Line {0}: Invalid signal/symbol name {1}.", lineno, sign); pause = true;
                                continue;
                            }
                        }

                        foreach (string sign in vals.Keys)
                        {
                            val |= (1 - reverse(vals[sign])) << sigbits[sign];
                        }

                        if (cmds[curr_cmd] < 0)
                        {
                            valFetch = val;
                        }
                        else
                        {
                            buffer[(cmds[curr_cmd] << 3) + 7 + beat] = val;
                        }

                        break;

                    default:
                        continue;
                }
            }

			buffer[0] = valFetch;
			
            foreach (string cmd in cmds.Keys)
            {
                if (cmds[cmd] < 0) continue;
                buffer[(cmds[cmd] << 3) + 7 + cmdBeatCount[cmd]] = valFetch;
            }

            foreach (string sig in sigrefs.Keys)
            {
                if (sigrefs[sig] == 0)
                {
                    Console.WriteLine("Warning: Signal {0} decleared but never used.", sig); pause = true;
                }
            }

            Console.WriteLine("Writing buffer...");

            if (m19file != "")
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m19file))
                {
                    for (int i = 0; i < 0x100; i += 4)
                    {
                        int checksum = 0x13 + (i % 256) + (i / 256) % 256;
                        for (int j = 0; j < 3; ++j)
                        {
                            checksum += (buffer[i + j] >> 24) + (buffer[i + j] >> 16) % 256 + (buffer[i + j] >> 8) % 256 + (buffer[i + j]) % 256;
                        }
                        checksum %= 256;
                        checksum = 0xFF - checksum;

                        sw.WriteLine("S113{0}{1}{2}{3}{4}{5}",
                                     i.ToString("X4"),
                                     buffer[i].ToString("X8"),
                                     buffer[i + 1].ToString("X8"),
                                     buffer[i + 2].ToString("X8"),
                                     buffer[i + 3].ToString("X8"),
                                     checksum.ToString("X2")
                                    );
                    }
                    sw.WriteLine("S9030000FC");
                }
            }

            if (vhdfile != "")
            {
				string[] vhdls = System.IO.File.ReadAllLines(vhdfile);
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(vhdfile))
                {
					for (int i = 0; i < vhdls.Length; ++i) {
						sw.WriteLine(vhdls[i]);
						if (vhdls[i] == "-- #MIRS") {
							while (vhdls[i] != "-- #/MIRS") ++i;
							foreach (string sn in sigbits.Keys)
							{
								sw.WriteLine(sn + "<=MIR(" + sigbits[sn] + ");");
							}
							sw.WriteLine("-- #/MIRS");
						}
						if (vhdls[i] == "-- #SIGS") {
							while (vhdls[i] != "-- #/SIGS") ++i;
							sw.Write("signal ");
							bool first = true;
							foreach (string sn in sigbits.Keys)
							{
								if (first) { first = false; sw.Write(sn); } else sw.Write(", " + sn);
							}
							sw.WriteLine(" : std_logic;");
							// sw.WriteLine("signal MIR : std_logic_vector({0} downto 0);", sigbits.Count);
							sw.WriteLine("-- #/SIGS");
						}
					}
                }
            }

            if (csvfile != "")
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(csvfile))
                {
                    sw.Write(",");
                    string[] fields = new string[sigbits.Keys.Count];
                    foreach (string sn in sigbits.Keys)
                    {
                        fields[sigbits[sn]] = "," + sn;
                    }
                    for (int i = 0; i < fields.Length; ++i)
                        sw.Write(fields[i]);
                    sw.WriteLine();

                    sw.Write("Valid:,");
                    fields = new string[sigbits.Keys.Count];
                    foreach (string sn in sigbits.Keys)
                    {
                        fields[sigbits[sn]] = "," + valids[sn];
                    }
                    for (int i = 0; i < fields.Length; ++i)
                        sw.Write(fields[i]);
                    sw.WriteLine();

                    sw.Write("Default:,");
                    fields = new string[sigbits.Keys.Count];
                    foreach (string sn in sigbits.Keys)
                    {
                        fields[sigbits[sn]] = "," + defaults[sn];
                    }
                    for (int i = 0; i < fields.Length; ++i)
                        sw.Write(fields[i]);
                    sw.WriteLine();

                    sw.WriteLine();

                    foreach (string cmd in cmds.Keys)
                    {
                        sw.Write("\"" + cmd + "\"");
                        for (int j = 0; j < cmdBeatCount[cmd]; ++j)
                        {
                            int val = 0;
                            fields = new string[sigbits.Keys.Count];
                            if (cmds[cmd] < 0) { val = valFetch; }
                            else
                            {
                                int i = (cmds[cmd] << 3) + 7 + j;
                                val = buffer[i];
                            } 
                            sw.Write("," + j);
                                
                            foreach (string sn in sigbits.Keys)
                            {
                                fields[sigbits[sn]] = "," + ((val >> sigbits[sn]) & 1);
                            }
                            for (int k = 0; k < fields.Length; ++k)
                                sw.Write(fields[k]);
                            sw.WriteLine();
                        }
                        sw.WriteLine();
                    }
                }
            }
			
			if (deffile != "") {
				byte[] defheader = new byte[] {0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
                using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.FileStream(deffile, System.IO.FileMode.Create), System.Text.Encoding.ASCII))
                {
                    bw.Write(defheader);
                    foreach (string cmd in cmds.Keys)
                    {
                        string[] cmdps = cmd.Split(' ');
                        bw.Write("-" + cmdps[0] + "\r\n");
                        for (int i = 1; i < cmdps.Length; ++i)
                        {
                            if (i > 1) bw.Write(", ");
                            bw.Write(cmdps[i]);
                        }
                        bw.Write("\r\n");
                        
                    }
                }
			}

            if (pause)
            {
                Console.Write("Press Enter to exit...");
                Console.ReadLine();
            }

            return;

        }
    }
}