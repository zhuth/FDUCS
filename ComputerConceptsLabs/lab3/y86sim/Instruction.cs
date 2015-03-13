using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim
{
    public enum ICodeFunc
    {
        NOP = 0x00, HALT = 0x10,
        RRMOVL = 0x20, IRMOVL = 0x30, RMMOVL = 0x40, MRMOVL = 0x50,
        ADDL = 0x60, SUBL = 0x61, ANDL = 0x62, XORL = 0x63, ORL = 0x64, MULL = 0x65, DIVL = 0x66, MODL = 0x67,
        SHRL = 0x68, SHLL = 0x69, SARL = 0x6A, OPL = 0x6F,
        JMP = 0x70, JLE = 0x71, JL = 0x72, JE = 0x73, JNE = 0x74, JGE = 0x75, JG = 0x76, JXX = 0x7F,
        CALL = 0x80, RET = 0x90, PUSHL = 0xA0, POPL = 0xB0
    }

    public enum Register
    {
        EAX = 0x0, ECX = 0x1, EDX = 0x2, EBX = 0X3,
        ESP = 0X4, EBP = 0X5, ESI = 0X6, EDI = 0X7,
        NONE = 0X8
    }

    [Flags]
    public enum ConditionCode
    {
        None, Zero, Overflow, Sign
    }

    public class Instruction
    {
        private ICodeFunc _icf, _hcf;

        public ICodeFunc ICodeOnly
        {
            get { return _hcf; }
        }

        public ICodeFunc CodeFunc
        {
            get { return _icf; }
            set { _icf = value; _hcf = Instruction.GetHigherType(_icf); }
        }
        private Register _ra, _rb;

        public Register RegisterB
        {
            get { return _rb; }
            set { _rb = value; }
        }

        public Register RegisterA
        {
            get { return _ra; }
            set { _ra = value; }
        }
        private int _imm;

        public int Immediate
        {
            get { return _imm; }
            set { _imm = value; }
        }

        private int _address;

        public int Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public Instruction(ICodeFunc cf, Register ra, Register rb, int imm)
        {
            _icf = cf; _ra = ra; _rb = rb; _imm = imm; _hcf = Instruction.GetHigherType(_icf);
        }

        public Instruction(MemoryBlock buffer, int address)
        {
            int i = address;
            ICodeFunc code = (ICodeFunc)buffer[i];
            Register ra = Register.NONE, rb = Register.NONE; int imm = 0;

            switch (Instruction.GetHigherType(code))
            {
                case ICodeFunc.RRMOVL:
                case ICodeFunc.OPL:
                    ra = (Register)buffer.GetHigherDigits(i + 1);
                    rb = (Register)buffer.GetLowerDigits(i + 1);
                    break;

                case ICodeFunc.JXX:
                case ICodeFunc.CALL:
                    imm = buffer.GetInt(i + 1);
                    break;

                case ICodeFunc.IRMOVL:
                    rb = (Register)buffer.GetLowerDigits(i + 1);
                    imm = buffer.GetInt(i + 2);
                    break;

                case ICodeFunc.RMMOVL:
                    ra = (Register)buffer.GetHigherDigits(i + 1);
                    rb = (Register)buffer.GetLowerDigits(i + 1);
                    imm = buffer.GetInt(i + 2);
                    break;

                case ICodeFunc.MRMOVL:
                    ra = (Register)buffer.GetHigherDigits(i + 1);
                    rb = (Register)buffer.GetLowerDigits(i + 1);
                    imm = buffer.GetInt(i + 2);
                    break;

                case ICodeFunc.PUSHL:
                case ICodeFunc.POPL:
                    ra = (Register)buffer.GetHigherDigits(i + 1);
                    break;

                default:
                    break;
            }

            _icf = code;
            _ra = ra;
            _rb = rb;
            _imm = imm;
            _hcf = GetHigherType(_icf);
            _address = address;
        }

        public override string ToString()
        {
            StringBuilder sw = new StringBuilder(_icf.ToString());
            switch (_hcf)
            {
                case ICodeFunc.RRMOVL:
                case ICodeFunc.OPL:
                    sw.AppendFormat(" %{0}, %{1}", _ra,_rb);
                    break;

                case ICodeFunc.JXX:
                case ICodeFunc.CALL:
                    sw.AppendFormat(" 0x{0}", _imm.ToString("x8"));
                    break;

                case ICodeFunc.IRMOVL:
                    sw.AppendFormat(" ${1}, %{0}", _rb, _imm);
                    break;

                case ICodeFunc.RMMOVL:
                    sw.AppendFormat(" %{0}, {2}(%{1})", _ra,_rb,_imm);
                    break;

                case ICodeFunc.MRMOVL:
                    sw.AppendFormat(" {2}(%{1}), %{0}", _ra, _rb, _imm);
                    break;

                case ICodeFunc.PUSHL:
                case ICodeFunc.POPL:
                    sw.AppendFormat(" %{0}", _ra);
                    break;

                default:
                    break;
            }

            return sw.ToString().ToLower();
        }

        /// <summary>
        /// not implemented
        /// </summary>
        /// <param name="instruct"></param>
        /// <returns></returns>
        public static Instruction FromString(string instruct)
        {
            instruct = instruct.Trim().ToUpper();
            string codename = Utility.SafeSubstringPos(instruct, 0, instruct.IndexOf(' ') - 1);
            ICodeFunc cf = (ICodeFunc)Enum.Parse(typeof(ICodeFunc), codename);
            instruct = Utility.SafeSubstring(instruct, instruct.IndexOf(' ') + 1);
            string[] reg = instruct.Split(',');

            throw new NotImplementedException();
        }

        public static ICodeFunc GetHigherType(ICodeFunc cf)
        {
            byte ins = (byte)cf;
            switch (ins >> 4)
            {
                case 0x7:
                    return ICodeFunc.JXX;
                case 0x6:
                    return ICodeFunc.OPL;
                default:
                    return cf;
            }
        }

        private static int[] InstrLength = new int[] { 1, 1, 2, 6, 6, 6, 2, 5, 5, 1, 2, 2 };
        public static int GetInstrLength(ICodeFunc cf)
        {
            return InstrLength[((int)cf & 0xf0) >> 4];
        }

    }
}
