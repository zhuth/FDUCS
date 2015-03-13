using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace y86sim
{
    static class Program
    {
        internal static Simulator sim;
        private static bool gui = true, disa = false;
        private static string filename = "", fileout = "", asmfile = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
//#if !DEBUG
            try
            {
//#endif
            for (int i = 0; i < argv.Length; ++i)
            {
                string arg = argv[i];
                if (arg.StartsWith("--")) arg = arg.Substring(1);
                if (arg.StartsWith("-fi:"))
                {
                    filename = arg.Substring("-fi:".Length);
                }
                if (arg == "-nogui")
                {
                    gui = false;
                }
                if (arg == "-disa")
                {
                    disa = true;
                }
                if (arg.StartsWith("-asm:"))
                {
                    asmfile = arg.Substring("-asm:".Length);
                }
                if (arg.StartsWith("-fo:"))
                {
                    fileout = arg.Substring("-fo:".Length);
                }
                if (arg == "-help")
                {
                    string myName = Application.ExecutablePath;
                    myName = myName.Substring(myName.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
                    myName = myName.Substring(0, myName.LastIndexOf('.'));
                    Console.WriteLine("Usage: {0} [option [option [...]]]\r\nOptions:", myName);
                    Console.WriteLine("\t-help              Show this help information.");
                    Console.WriteLine("\t-fi:<input file>   Specify the binary program to run.");
                    Console.WriteLine("\t-fo:<output file>  Specify the output text file for mediate results.");
                    Console.WriteLine("\t-disa              Disassembly the program.");
                    Console.WriteLine("\t-asm:<asm file>    Assemble the specified file.");
                    Console.WriteLine("\t-nogui             Run program without showing a graphic interface.");
                    Console.WriteLine("");
                    Console.WriteLine("朱恬骅 09300240004");

                    if (argv.Contains("-pause"))
                    {
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey(false);
                    }
                    return;
                }
            }
            if (asmfile != "")
            {
                if (fileout == "") fileout = Utility.ChangeExtName(asmfile, "yo");
                Assembler asm = new Assembler(System.IO.File.ReadAllLines(asmfile));
                asm.Assemble(fileout);
                return;
            }
            if (filename == "")
                gui = true;
            else
            {
                if (fileout == "") fileout = Utility.ChangeExtName(filename, "txt");
                if (disa)
                {
                    InstructionList instructions = new InstructionList(new MemoryBlock(System.IO.File.ReadAllBytes(filename)));
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(Utility.ChangeExtName(fileout, "asm"));
                    foreach (Instruction i in instructions)
                    {
                        Console.WriteLine("0x{0}:\t{1}", (i.Address + Properties.Settings.Default.ProgramLoadMem).ToString("x8"), i);
                        sw.WriteLine(i);
                    }
                    sw.Close();
                    return;
                }
                initSimulator();
            }
            if (gui)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            else
            {
                sim.Run();
                while (true) ;// System.Threading.Thread.Sleep(10000);
            }
//#if DEBUG
            }
            catch (Exception ex)
            {
                Utility.PromptException(ex);
                if (gui) 
                    MessageBox.Show(ex.Message, "y86sim", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
//#endif
        }

        private static void initSimulator()
        {
            if (Program.sim == null)
            {
                Program.sim = new Simulator(filename, fileout, Properties.Settings.Default.ProgramLoadMem, Properties.Settings.Default.StackLoadMem);
                sim.JmpUseRelativeAddress = Properties.Settings.Default.JmpRelativePosition;
                sim.OnSimulatorHalt += new ProcessorHaltEventHandler(sim_OnSimulatorHalt);
                sim.OnClockCycle += new Action(sim_OnClockCycle);
            }
            else
                Program.sim.Initialize();
        }

        private static int count = 0;
        private static void sim_OnClockCycle()
        {
            ++count;
            Console.WriteLine("Clock cycle {0}.", count);
        }

        private static void sim_OnSimulatorHalt(int eax)
        {
            Console.WriteLine("Simulator halt. EAX = {0}", eax);
            if (gui) initSimulator(); else Environment.Exit(0);
        }
    }
}
