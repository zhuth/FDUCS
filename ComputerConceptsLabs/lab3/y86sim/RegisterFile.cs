using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace y86sim
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class RegisterFile
    {
        private int[] _registers = new int[8];

        public RegisterFile() {
        }

        public int this[Register reg]
        {
            get
            {
                if ((int)reg >= _registers.Length) return 0;
                return _registers[(int)reg];
            }
            set {
                if ((int)reg >= _registers.Length) return;
                _registers[(int)reg] = value;
            }
        }

        public int EAX { get { return this[Register.EAX]; } set { this[Register.EAX] = value; } }
        public int EBX { get { return this[Register.EBX]; } set { this[Register.EBX] = value; } }
        public int ECX { get { return this[Register.ECX]; } set { this[Register.ECX] = value; } }
        public int EDX { get { return this[Register.EDX]; } set { this[Register.EDX] = value; } }
        public int EBP { get { return this[Register.EBP]; } set { this[Register.EBP] = value; } }
        public int ESP { get { return this[Register.ESP]; } set { this[Register.ESP] = value; } }
        public int ESI { get { return this[Register.ESI]; } set { this[Register.ESI] = value; } }
        public int EDI { get { return this[Register.EDI]; } set { this[Register.EDI] = value; } }

        public int this[byte reg, bool higher]
        {
            get
            {
                if (higher) reg = (byte)(reg >> 4);
                return _registers[reg & 0xf];
            }
            set
            {
                if (higher) reg = (byte)(reg >> 4);
                _registers[reg & 0xf] = value;
            }
        }
    }
}
