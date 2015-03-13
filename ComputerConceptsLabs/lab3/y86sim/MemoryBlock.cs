using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace y86sim
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class MemoryBlock
    {
        private byte[] _buffer;

        public byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public int Size
        {
            get { return _buffer.Length; }
            set
            {
                if (value < 0) return;
                int len = Math.Min(value, _buffer.Length);
                byte[] nd = new byte[value];
                for (int i = 0; i < len; ++i)
                    nd[i] = _buffer[i];
                _buffer = nd;
            }
        }

        public MemoryBlock(int size)
        {
            _buffer = new byte[size];
        }

        public byte this[int index]
        {
            get
            {
#if !DEBUG            
                if (index < 0 || index > _buffer.Length)
                {

                Console.WriteLine(new Exception("Out of memory block.")); 

                    return 0;
                } else
#endif
                return _buffer[index];
            }
            set { _buffer[index] = value; }
        }
        
        public MemoryBlock(byte[] bytes) {
            _buffer = bytes;
        }

        public int GetInt(int offset)
        {
            if (offset >= _buffer.Length)
            {
                throw new Exception("Memory visit error: invalid address 0x" + offset.ToString("x8") + ".");
            }
            return (_buffer[offset] << 24) | (_buffer[offset + 1] << 16) | (_buffer[offset + 2] << 8) | _buffer[offset + 3];
        }

        public byte GetHigherDigits(int offset)
        {
            return (byte)((_buffer[offset] & 0xf0) >> 4);
        }

        public byte GetLowerDigits(int offset)
        {
            return (byte)(_buffer[offset] & 0x0f);
        }

        public void PutInt(int destOffset, int val)
        {
            byte a = (byte)(val >> 24), b = (byte)(val >> 16), c = (byte)(val >> 8), d = (byte)val;
            _buffer[destOffset] = a;
            _buffer[destOffset + 1] = b;
            _buffer[destOffset + 2] = c;
            _buffer[destOffset + 3] = d; 
        }

        public Instruction GetInstruction(int address)
        {
            return new Instruction(this, address);
        }

        public void Load(byte[] buffer, int programLoadAddr)
        {
            for (int i = 0; i < buffer.Length && i + programLoadAddr < _buffer.Length; ++i) 
                _buffer[i + programLoadAddr] = buffer[i];
        }
    }
}
