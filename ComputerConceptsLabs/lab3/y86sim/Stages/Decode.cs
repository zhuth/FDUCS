using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim.Stages
{
    public class Decode : Stage
    {
        Register _ra = Register.NONE, _rb = Register.NONE;

        public Register Rb
        {
            get { return _rb; }
            set { _rb = value; }
        }

        public Register Ra
        {
            get { return _ra; }
            set { _ra = value; }
        }

        int _valC = 0, _valP = 0;

        public int ValP
        {
            get { return _valP; }
            set { _valP = value; }
        }

        public int ValC
        {
            get { return _valC; }
            set { _valC = value; }
        }

        int sig_d_valA = 0, sig_d_valB = 0;
        Register sig_d_srcA = Register.NONE, sig_d_srcB = Register.NONE, sig_d_dstE = Register.NONE, sig_d_dstM = Register.NONE;

        public Register Sig_d_dstM
        {
            get { return sig_d_dstM; }
            set { sig_d_dstM = value; }
        }

        public Register Sig_d_dstE
        {
            get { return sig_d_dstE; }
            set { sig_d_dstE = value; }
        }

        public Register Sig_d_srcB
        {
            get { return sig_d_srcB; }
            set { sig_d_srcB = value; }
        }

        public Register Sig_d_srcA
        {
            get { return sig_d_srcA; }
            set { sig_d_srcA = value; }
        }

        public int Sig_d_valB
        {
            get { return sig_d_valB; }
            set { sig_d_valB = value; }
        }

        public int Sig_d_valA
        {
            get { return sig_d_valA; }
            set { sig_d_valA = value; }
        }

        public void Update()
        {
            base.Update(() => {
                Instruction = Processor.F.Instruction;
                _icode = Processor.F.Instruction.CodeFunc;
                _ra = Processor.F.Instruction.RegisterA;
                _rb = Processor.F.Instruction.RegisterB;
                _valC = Processor.F.Sig_valC;
                _valP = Processor.F.Sig_valP;
            });

            ICodeFunc icode = Instruction.GetHigherType(ICode);

            sig_d_dstE = sig_d_dstM = sig_d_srcA = sig_d_srcB = Register.NONE;

            if (icode == ICodeFunc.OPL || icode == ICodeFunc.RRMOVL || icode == ICodeFunc.RMMOVL || icode == ICodeFunc.PUSHL)
                sig_d_srcA = _ra;
            else if (icode == ICodeFunc.POPL || icode == ICodeFunc.RET)
                sig_d_srcA = Register.ESP;
            else
                sig_d_srcA = Register.NONE;

            if (icode == ICodeFunc.OPL || icode == ICodeFunc.RMMOVL || icode == ICodeFunc.MRMOVL)
                sig_d_srcB = _rb;
            else if (icode == ICodeFunc.PUSHL || icode == ICodeFunc.POPL || icode == ICodeFunc.CALL || icode == ICodeFunc.RET)
                sig_d_srcB = Register.ESP;
            else
                sig_d_srcB = Register.NONE;

            if (icode == ICodeFunc.OPL || icode == ICodeFunc.IRMOVL || icode == ICodeFunc.RRMOVL)
                sig_d_dstE = _rb;
            else if (icode == ICodeFunc.PUSHL || icode == ICodeFunc.POPL || icode == ICodeFunc.CALL || icode == ICodeFunc.RET)
                sig_d_dstE = Register.ESP;
            else
                sig_d_dstE = Register.NONE;

            if (icode == ICodeFunc.MRMOVL || icode == ICodeFunc.POPL)
                sig_d_dstM = _ra;
            else
                sig_d_dstM = Register.NONE;

            if (icode == ICodeFunc.CALL || icode == ICodeFunc.JXX)
                sig_d_valA = _valP;
            else if (sig_d_srcA == Processor.E.DstE)
                sig_d_valA = Processor.E.Sig_e_valE;
            else if (sig_d_srcA == Processor.M.DstM)
                sig_d_valA = Processor.M.Sig_m_valM;
            else if (sig_d_srcA == Processor.M.DstE)
                sig_d_valA = Processor.M.ValE;
            else if (sig_d_srcA == Processor.W.DstM)
                sig_d_valA = Processor.W.ValM;
            else if (sig_d_srcA == Processor.W.DstE)
                sig_d_valA = Processor.W.ValE;
            else
                sig_d_valA = Processor.RegisterFile[sig_d_srcA];

            if (sig_d_srcB == Processor.E.DstE)
                sig_d_valB = Processor.E.Sig_e_valE;
            else if (sig_d_srcB == Processor.M.DstM)
                sig_d_valB = Processor.M.Sig_m_valM;
            else if (sig_d_srcB == Processor.M.DstE)
                sig_d_valB = Processor.M.ValE;
            else if (sig_d_srcB == Processor.W.DstM)
                sig_d_valB = Processor.W.ValM;
            else if (sig_d_srcB == Processor.W.DstE)
                sig_d_valB = Processor.W.ValE;
            else
                sig_d_valB = Processor.RegisterFile[sig_d_srcB];
            
        }

        protected override void doBubble()
        {
            _icode = ICodeFunc.NOP; _ra = Register.NONE; _rb = Register.NONE; _valC = 0;
        }

        public Decode(Processor proc)
        {
            Processor = proc;
        }

    }
}
