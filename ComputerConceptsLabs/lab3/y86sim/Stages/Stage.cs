using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace y86sim
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter ))]
    public class Stage
    {
        private bool _stall = false, _bubble = false;
        protected ICodeFunc _icode = ICodeFunc.NOP;
        protected Processor _proc;
        private Instruction _instruction = new Instruction(ICodeFunc.NOP, Register.NONE, Register.NONE, 0);

        public Instruction Instruction
        {
            get { return _instruction; }
            set { _instruction = value; _icode = _instruction.CodeFunc; }
        }

        public Processor Processor
        {
            get { return _proc; }
            set { _proc = value; }
        }

        public ICodeFunc ICode
        {
            get { return _instruction.CodeFunc; }
            set { _icode = _instruction.CodeFunc = value; }
        }

        public bool Bubble
        {
            get { return _bubble; }
            set { _bubble = value; }
        }

        public bool Stall
        {
            get { return _stall; }
            set { _stall = value; }
        }

        public Stage(Processor processor)
        {
            _proc = processor;
        }

        protected Stage() { }

        protected virtual void doBubble() { }
        
        public virtual void Update(Action action)
        {
            if (_bubble)
            {
                _instruction = new Instruction(ICodeFunc.NOP, Register.NONE, Register.NONE, 0);
                doBubble();
                return;
            }
            if (_stall) return;
            if (action != null) action();
        }
    }
}
