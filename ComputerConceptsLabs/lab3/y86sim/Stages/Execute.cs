using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim.Stages
{
    public class Execute : Stage
    {
        private int _valC = 0, _valA = 0, _valB = 0;
        private Register _dstE = 0, _dstM = 0, _srcA = 0, _srcB = 0;
        private int sig_e_valE = 0, sig_e_valA = 0;

        public int Sig_e_valA
        {
            get { return sig_e_valA; }
            set { sig_e_valA = value; }
        }

        public int Sig_e_valE
        {
            get { return sig_e_valE; }
            set { sig_e_valE = value; }
        }

        private bool sig_e_bch = false;

        public bool Sig_e_bch
        {
            get { return sig_e_bch; }
            set { sig_e_bch = value; }
        }

        private ConditionCode cc = ConditionCode.None;

        public ConditionCode ConditionCodes
        {
            get { return cc; }
            set { cc = value; }
        }        

        public Register SrcB
        {
            get { return _srcB; }
            set { _srcB = value; }
        }

        public Register SrcA
        {
            get { return _srcA; }
            set { _srcA = value; }
        }

        public Register DstM
        {
            get { return _dstM; }
            set { _dstM = value; }
        }

        public Register DstE
        {
            get { return _dstE; }
            set { _dstE = value; }
        }

        public int ValB
        {
            get { return _valB; }
            set { _valB = value; }
        }

        public int ValA
        {
            get { return _valA; }
            set { _valA = value; }
        }

        public int ValC
        {
            get { return _valC; }
            set { _valC = value; }
        }

        public Execute(Processor proc) { Processor = proc; }

        protected override void doBubble()
        {
            _icode = ICodeFunc.NOP; _valA = _valC = _valB = 0; _dstE = _dstM = _srcA = _srcB = Register.NONE;
        }

        public void Update()
        {
            base.Update(() =>
            {
                Instruction = Processor.D.Instruction;
                _icode = Processor.D.ICode;
                _valC = Processor.D.ValC;
                _valA = Processor.D.Sig_d_valA;
                _valB = Processor.D.Sig_d_valB;
                _dstE = Processor.D.Sig_d_dstE;
                _dstM = Processor.D.Sig_d_dstM;
                _srcA = Processor.D.Sig_d_srcA;
                _srcB = Processor.D.Sig_d_srcB;
                sig_e_valA = _valA;
            });

            int aluA = 0;
            switch (Instruction.GetHigherType(ICode))
            {
                case ICodeFunc.RRMOVL:
                case ICodeFunc.OPL:
                    aluA = _valA;
                    break;

                case ICodeFunc.IRMOVL:
                case ICodeFunc.RMMOVL:
                case ICodeFunc.MRMOVL:
                    aluA = _valC;
                    break;

                case ICodeFunc.CALL:
                case ICodeFunc.PUSHL:
                    aluA = -4;
                    break;

                case ICodeFunc.RET:
                case ICodeFunc.POPL:
                    aluA = 4;
                    break;
            }

            int aluB = 0;
            switch (Instruction.GetHigherType(ICode))
            {
                case ICodeFunc.RMMOVL:
                case ICodeFunc.MRMOVL:
                case ICodeFunc.OPL:
                case ICodeFunc.CALL:
                case ICodeFunc.PUSHL:
                case ICodeFunc.RET:
                case ICodeFunc.POPL:
                    aluB = _valB;
                    break;

                case ICodeFunc.RRMOVL:
                case ICodeFunc.IRMOVL:
                    aluB = 0;
                    break;
            }

            bool overflow = false;
            switch (ICode)
            {
                case ICodeFunc.ANDL:
                    sig_e_valE = aluB & aluA;
                    break;

                case ICodeFunc.XORL:
                    sig_e_valE = aluB ^ aluA;
                    break;
                    
                case ICodeFunc.ORL :
                    sig_e_valE = aluB | aluA;
                    break;

                case ICodeFunc.MULL:
                    sig_e_valE = aluB * aluA;
                    overflow = ((Int64)aluA * (Int64)aluB > int.MaxValue);
                    break;

                case ICodeFunc.DIVL:
                    sig_e_valE = aluB / aluA;
                    break;

                case ICodeFunc.MODL:
                    sig_e_valE = aluB % aluA;
                    break;

                case ICodeFunc.SHRL:
                    sig_e_valE = aluB >> aluA;
                    break;

                case ICodeFunc.SHLL:
                    sig_e_valE = aluB << aluA;
                    break;

                case ICodeFunc.SARL:
                    sig_e_valE = (int)((uint)aluB >> aluA);
                    break;

                case ICodeFunc.SUBL:
                    sig_e_valE = aluB - aluA;
                    overflow = ((Int64)aluB - (Int64)aluA > int.MaxValue) || ((Int64)aluB - (Int64)aluA < int.MinValue);                    
                    break;
                
                default:
                    sig_e_valE = aluB + aluA;
                    overflow = ((Int64)aluB + (Int64)aluA > int.MaxValue) || ((Int64)aluB + (Int64)aluA < int.MinValue);
                    break;
            }

            switch (Instruction.GetHigherType(ICode))
            {
                case ICodeFunc.OPL:
                    // update CC
                    cc = ConditionCode.None;
                    if (sig_e_valE < 0) cc |= ConditionCode.Sign;
                    if (sig_e_valE == 0) cc |= ConditionCode.Zero;
                    if (overflow) cc |= ConditionCode.Overflow;
                    break;
                    
                case ICodeFunc.JXX:
                    bool zf = (cc & ConditionCode.Zero) == ConditionCode.Zero,
                         of = (cc & ConditionCode.Overflow) == ConditionCode.Overflow,
                         sf = (cc & ConditionCode.Sign) == ConditionCode.Sign;
                    switch (ICode)
                    {
                        case ICodeFunc.JE:
                            sig_e_bch = zf;
                            break;

                        case ICodeFunc.JNE:
                            sig_e_bch = !zf;
                            break;

                        case ICodeFunc.JL:
                            sig_e_bch = sf ^ of;
                            break;

                        case ICodeFunc.JG:
                            sig_e_bch = (!(sf ^ of)) && zf;
                            break;

                        case ICodeFunc.JLE:
                            sig_e_bch = (sf ^ of) || zf;
                            break;

                        case ICodeFunc.JGE:
                            sig_e_bch = !(sf ^ of);
                            break;

                        case ICodeFunc.JMP:
                            sig_e_bch = true;
                            break;
                    }
                    break;
            }
        }

    }
}
