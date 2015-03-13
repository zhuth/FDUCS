using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim
{
    public class InstructionList
    {
        private Instruction[] Instructions;
        private int[] _addresses;
        private int _addressOffset, _addressIncrement;

        public InstructionList(MemoryBlock buffer, int offset = 0, int addressOffset = 0x400, int addressIncrement = 1)
        {
            List<Instruction> list = new List<Instruction>();
            _addresses = new int[buffer.Size];
            for (int i = offset; i < buffer.Size; )
            {
                list.Add(new Instruction(buffer, i));
                list[list.Count - 1].Address = i;
                _addresses[i] = list.Count - 1;
                i += Instruction.GetInstrLength(list[list.Count-1].CodeFunc);
            }

            Instructions = list.ToArray();
            _addressIncrement = addressIncrement;
            _addressOffset = addressOffset;
        }

        private int getInstructionIndex(int address)
        {
            return _addresses[(address - _addressOffset) * _addressIncrement];
        }

        public Instruction this[int address]
        {
            get
            {
                return Instructions[getInstructionIndex(address)];
            }

            set
            {
                Instructions[getInstructionIndex(address)] = value;
            }
        }

        public IEnumerator<Instruction> GetEnumerator()
        {
            foreach (Instruction i in Instructions)
                yield return i;
        }
    }
}
