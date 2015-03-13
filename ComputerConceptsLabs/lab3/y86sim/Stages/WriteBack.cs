using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim.Stages
{
    public class WriteBack : Stage
    {
        private int _valE=0, _valM = 0;
        private Register _dstE = Register.NONE, _dstM = Register.NONE;

        public int ValE
        {
            get { return _valE; }
            set { _valE = value; }
        }

        public int ValM
        {
            get { return _valM; }
            set { _valM = value; }
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
        
        protected override void doBubble()
        {
            _icode = ICodeFunc.NOP; _valE = _valM = 0; _dstE = _dstM = Register.NONE;
        }

        public void Update()
        {
            base.Update(() => {
                Instruction = Processor.M.Instruction;
                _icode = Processor.M.ICode;
                _valE = Processor.M.ValE;
                _valM = Processor.M.Sig_m_valM;
                _dstE = Processor.M.DstE;
                _dstM = Processor.M.DstM;
            });

            Processor.RegisterFile[_dstE] = _valE;
            Processor.RegisterFile[_dstM] = _valM;
            
            if (ICode == ICodeFunc.HALT) Processor.Halt();
        }

        public WriteBack(Processor proc) { Processor = proc; }
    }
}

