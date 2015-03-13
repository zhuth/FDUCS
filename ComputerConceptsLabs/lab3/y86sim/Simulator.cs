using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace y86sim
{
    public class Simulator
    {
        private string _fi, _fo;
        private int _loadAddr, _stackAddr;

        private MemoryBlock mem = new MemoryBlock(Properties.Settings.Default.MemorySize);
        private Processor proc;

        private int cycleNum = 0;
        private StreamWriter swFo;

        public event ProcessorHaltEventHandler OnSimulatorHalt;
        public event Action OnClockCycle;

        public bool JmpUseRelativeAddress
        {
            get { return proc.JmpUseRelativeAddress; }
            set { proc.JmpUseRelativeAddress = value; }
        }

        public Processor Processor
        {
            get { return proc; }
        }

        public Simulator(string fi, string fo, int loadAddr = 0x400, int stackAddr = 0x400)
        {
            Initialize(fi, fo, loadAddr, stackAddr);
        }

        void proc_OnHalt(int eax)
        {
            cycleNum = 0;
            if (OnSimulatorHalt != null) OnSimulatorHalt(eax);
            if (swFo != null) { swFo.Close(); swFo = null; }
        }

        public void Run()
        {
            cycleNum = 0;
            proc.Clock.Start();
        }

        public int Step()
        {
            proc.Clock.Tick();
            return proc.RegisterFile[Register.EAX];
        }

        private const string STR_FORMAT = "Circle_{0}\r\n--------------------\r\nFETCH:\r\n\tF_predPC \t=\t{1}\r\n\r\nDECODE" +
                                            ":\r\n\tD_icode \t=\t{2}\r\n\tD_ifun \t\t=\t{3}\r\n\tD_rA\t\t=\t{4}\r\n\tD_rB\t\t" +
                                            "=\t{5}\r\n\tD_valC\t\t=\t{6}\r\n\tD_valP\t\t=\t{7}\r\n\r\nEXECUTE:\r\n\tE_icode\t\t=\t{8" +
                                            "}\r\n\tE_ifun\t\t=\t{9}\r\n\tE_valC\t\t=\t{10}\r\n\tE_valA\t\t=\t{11}\r\n\tE_valB\t\t=\t" +
                                            "{12}\r\n\tE_dstE\t\t=\t{13}\r\n\tE_dstM\t\t=\t{14}\r\n\tE_srcA\t\t=\t{15}\r\n\tE_srcB\t" +
                                            "\t=\t{16}\r\n    \r\nMEMORY:\r\n\tM_icode\t\t=\t{17}\r\n\tM_Bch\t\t=\t{18}\r\n\tM_v" +
                                            "alE\t\t=\t{19}\r\n\tM_valA\t\t=\t{20}\r\n\tM_dstE\t\t=\t{21}\r\n\tM_dstM\t\t=\t{22}\r\n" +
                                            "\r\nWRITE BACK:\r\n\tW_icode\t\t=\t{23}\r\n\tW_valE\t\t=\t{24}\r\n\tW_valM\t\t=\t{25}\r" +
                                            "\n\tW_dstE\t\t=\t{26}\r\n\tW_dstM\t\t=\t{27}\r\n";

        void proc_OnAfterClock()
        {
            ++cycleNum;
            try
            {
                if (swFo != null)
                swFo.WriteLine(STR_FORMAT, cycleNum,
                    proc.F.PredPC,
                    (int)proc.D.ICode >> 4, (int)proc.D.ICode & 0xf, (int)proc.D.Ra, (int)proc.D.Rb, proc.D.ValC, proc.D.ValP,
                    (int)proc.E.ICode >> 4, (int)proc.E.ICode & 0xf, proc.E.ValC, proc.E.ValA, proc.E.ValB, (int)proc.E.DstE, (int)proc.E.DstM, (int)proc.E.SrcA, (int)proc.E.SrcB,
                    (int)proc.M.ICode >> 4, proc.M.Bch ? 1 : 0, proc.M.ValE, proc.M.ValA, (int)proc.M.DstE, (int)proc.M.DstM,
                    (int)proc.W.ICode >> 4, proc.W.ValE, proc.W.ValM, (int)proc.W.DstE, (int)proc.W.DstM
                    );
            }
            catch { }
            if (OnClockCycle != null) OnClockCycle();
        }


        public void Initialize(bool writeFile = false)
        {
            Initialize(_fi, writeFile ? _fo : null, _loadAddr, _stackAddr);
        }

        public void Initialize(string fi, string fo, int loadAddr = 0x400, int stackAddr = 0x400)
        {
            if (swFo != null) try { swFo.Close(); swFo = null; }
                catch { }
            _fi = fi; _loadAddr = loadAddr; _stackAddr = stackAddr;
            if (fo != null) { swFo = new StreamWriter(fo); _fo = fo; }
            byte[] buffer = File.ReadAllBytes(fi);
            mem = new MemoryBlock(Properties.Settings.Default.MemorySize);
            proc = new Processor(mem);
            mem.Load(buffer, loadAddr);
            if (Properties.Settings.Default.UseEOPTag) mem.Buffer[loadAddr + buffer.Length] = (byte)ICodeFunc.HALT;
            // proc.OnBeforeClock += new Action(proc_OnBeforeClock);
            proc.OnAfterClock += new Action(proc_OnAfterClock);
            proc.OnHalt += new ProcessorHaltEventHandler(proc_OnHalt);
            proc.RegisterFile[Register.ESP] = proc.RegisterFile[Register.EBP] = stackAddr;
            proc.F.Instruction = new Instruction(ICodeFunc.NOP, Register.NONE, Register.NONE, 0);
            proc.F.Sig_pred_pc = loadAddr;
        }

        public void Stop()
        {
            proc.Clock.Stop();
            if (OnSimulatorHalt != null) OnSimulatorHalt(proc.RegisterFile.EAX);
        }
    }
}
