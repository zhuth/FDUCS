using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim.Stages
{
    public class Fetch : Stage
    {
        private int _predPC = 0;
        private int sig_valC, sig_valP, sig_pred_pc;

        public int Sig_pred_pc
        {
            get { return sig_pred_pc; }
            set { sig_pred_pc = value; }
        }

        public int Sig_valP
        {
            get { return sig_valP; }
            set { sig_valP = value; }
        }

        public int Sig_valC
        {
            get { return sig_valC; }
            set { sig_valC = value; }
        }

        public int PredPC
        {
            get { return _predPC; }
            set { _predPC = value; }
        }


        private bool _jmpUseRelativeAddress;

        public bool JmpUseRelativeAddress
        {
            get { return _jmpUseRelativeAddress; }
            set { _jmpUseRelativeAddress = value; }
        }

        public void Update()
        {
            base.Update(() =>
            {
                _predPC = sig_pred_pc;
            });
            DoFetch();
        }

        internal void DoFetch(Instruction inst = null)
        {
            if (inst == null)
                Instruction = Processor.MemoryBlock.GetInstruction(_predPC);
            else
                Instruction = inst;

            if (Instruction.GetHigherType(Processor.M.ICode) == ICodeFunc.JXX && !Processor.M.Bch)
                Instruction = Processor.MemoryBlock.GetInstruction(_predPC = Processor.M.ValA);
            else if (Processor.W.ICode == ICodeFunc.RET) sig_pred_pc = Processor.W.ValM;
            else sig_pred_pc = _predPC;

            sig_valC = Instruction.Immediate;
            sig_valP = _predPC + y86sim.Instruction.GetInstrLength(Instruction.CodeFunc);

            if (Instruction.ICodeOnly == ICodeFunc.JXX || Instruction.ICodeOnly == ICodeFunc.CALL) sig_pred_pc = sig_valC;
            else sig_pred_pc = sig_valP;

            if ((Instruction.ICodeOnly == ICodeFunc.JXX || Instruction.ICodeOnly == ICodeFunc.CALL) && _jmpUseRelativeAddress) sig_pred_pc = sig_valP + sig_valC;
        }

        protected override void doBubble()
        {
            Instruction = new Instruction(ICodeFunc.NOP, Register.NONE, Register.NONE, 0);
        }

        public Fetch(Processor proc) { Processor = proc; }
    }
}
