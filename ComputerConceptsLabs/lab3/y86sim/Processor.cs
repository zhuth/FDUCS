using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace y86sim
{
    public delegate void ProcessorHaltEventHandler(int eax);

    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class Processor
    {
        public Stages.Decode D;
        public Stages.Execute E;
        public Stages.Fetch F;
        public Stages.Memory M;
        public Stages.WriteBack W;
        public RegisterFile RegisterFile = new RegisterFile();
        public MemoryBlock MemoryBlock;
        public Clock Clock = new Clock(Properties.Settings.Default.ClockInterval);
        
        public event Action OnBeforeClock;
        public event Action OnAfterClock;
        public event ProcessorHaltEventHandler OnHalt;

        public bool JmpUseRelativeAddress
        {
            get { return F.JmpUseRelativeAddress; }
            set { F.JmpUseRelativeAddress = value; }
        }

        public MemoryBlock MemoryContent { get { return MemoryBlock; } }
        public Stages.Memory MemoryStage { get { return M; } }
        public Stages.WriteBack WriteBackStage { get { return W; } }
        public Stages.Fetch FetchStage { get { return F; } }
        public Stages.Execute ExecuteStage { get { return E; } }
        public Stages.Decode DecodeStage { get { return D; } }


        public Processor(MemoryBlock mem)
        {
            M = new Stages.Memory(this);
            D = new Stages.Decode(this);
            E = new Stages.Execute(this);
            F = new Stages.Fetch(this);
            W = new Stages.WriteBack(this);
            Clock.OnTick += new Action(Cycle);
            MemoryBlock = mem;
        }

        public void Cycle()
        {
            try
            {
                if (OnBeforeClock != null) OnBeforeClock();

                // Set stall and bubble status
                var eicode = E.Instruction.ICodeOnly;

                F.Stall = (eicode == ICodeFunc.MRMOVL || eicode == ICodeFunc.POPL) && (E.DstM == D.Sig_d_srcA || E.DstM == D.Sig_d_srcB) || (D.ICode == ICodeFunc.RET || eicode == ICodeFunc.RET || M.ICode == ICodeFunc.RET);

                D.Stall = (eicode == ICodeFunc.MRMOVL || eicode == ICodeFunc.POPL) && (E.DstM == D.Sig_d_srcA || E.DstM == D.Sig_d_srcB);
                D.Bubble = (eicode == ICodeFunc.JXX && !E.Sig_e_bch) ||
                    (D.ICode == ICodeFunc.RET || eicode == ICodeFunc.RET || M.ICode == ICodeFunc.RET);

                E.Bubble = (eicode == ICodeFunc.JXX && !E.Sig_e_bch) ||
                    ((eicode == ICodeFunc.MRMOVL || eicode == ICodeFunc.POPL) && (E.DstM == D.Sig_d_srcA || E.DstM == D.Sig_d_srcB));

                W.Update(); M.Update(); E.Update(); D.Update(); F.Update();

                if (OnAfterClock != null) OnAfterClock();

                if (W.ICode == ICodeFunc.NOP && M.ICode == ICodeFunc.NOP && E.ICode == ICodeFunc.NOP && D.ICode == ICodeFunc.NOP && F.ICode == ICodeFunc.NOP && F.PredPC < 0)
                    Halt();
            }  
            catch (Exception ex)
            {
                Utility.PromptException(ex);
                System.Windows.Forms.MessageBox.Show(ex.Message, "y86sim",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Halt();
            }
        }

        public void Halt()
        {
            Clock.Stop();
            if (OnHalt != null) OnHalt(RegisterFile[Register.EAX]);
        }
    }
}
