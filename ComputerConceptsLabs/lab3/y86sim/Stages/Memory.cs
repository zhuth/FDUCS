using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim.Stages
{
    public class Memory : Stage
    {
        private bool _bch;
        private int _valE, _valA;
        private Register _dstE, _dstM;
        private int sig_m_valM;

        public int Sig_m_valM
        {
            get { return sig_m_valM; }
            set { sig_m_valM = value; }
        }

        public int ValE
        {
            get { return _valE; }
            set { _valE = value; }
        }

        public int ValA
        {
            get { return _valA; }
            set { _valA = value; }
        }

        public Register DstE
        {
            get { return _dstE; }
            set { _dstE = value; }
        }

        public Register DstM
        {
            get { return _dstM; }
            set { _dstM = value; }
        }

        public bool Bch
        {
            get { return _bch; }
            set { _bch = value; }
        }

        protected override void doBubble()
        {
            _icode = ICodeFunc.NOP; _valA = _valE = 0; _dstE = _dstM = Register.NONE;
        }

        public void Update()
        {
            base.Update(() =>
            {
                Instruction = Processor.E.Instruction;
                _icode = Processor.E.ICode;
                _bch = Processor.E.Sig_e_bch;
                _valE = Processor.E.Sig_e_valE;
                _valA = Processor.E.Sig_e_valA;
                _dstE = Processor.E.DstE;
                _dstM = Processor.E.DstM;
            });

            int mem_addr = 0;
            switch (ICode)
            {
                case ICodeFunc.RMMOVL:
                case ICodeFunc.PUSHL:
                case ICodeFunc.CALL:
                case ICodeFunc.MRMOVL:
                    mem_addr = _valE;
                    break;
                case ICodeFunc.POPL:
                case ICodeFunc.RET:
                    mem_addr = _valA;
                    break;
            }

            bool mem_read = ICode == ICodeFunc.MRMOVL || ICode == ICodeFunc.POPL || ICode == ICodeFunc.RET;
            bool mem_write = ICode == ICodeFunc.RMMOVL || ICode == ICodeFunc.PUSHL || ICode == ICodeFunc.CALL;

            if (mem_read) sig_m_valM = Processor.MemoryBlock.GetInt(mem_addr);
            if (mem_write) Processor.MemoryBlock.PutInt(mem_addr, _valA);
        }

        public Memory(Processor proc) { Processor = proc; }
    }
}
