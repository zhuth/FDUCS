using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace y86sim
{
    public class Assembler
    {
        private string[] _lines;
        private int[] _binpos;
        private Dictionary<string, int> _labels = new Dictionary<string, int>();
        private Regex regSpacer = new Regex(@"\s+");
        //private List<Instruction> _instrs = new List<Instruction>();
        private MemoryBlock mem = new MemoryBlock(0x1000);

        public Assembler(string[] srcLines)
        {
            _lines = srcLines;
            _binpos = new int[srcLines.Length];
        }

        private int generateBin(bool labelRun) {
            int pos = 0;
            for (int ln = 0; ln < _lines.Length; ++ln)
            {
                _binpos[ln] = pos;
                string line = _lines[ln].ToLower();
                line = regSpacer.Replace(line, " ");
                if (line.IndexOf('#') > 0) line = line.Substring(0, line.IndexOf('#'));
                if (line.IndexOf(":") > 0) // a label
                {
                    if (labelRun) _labels.Add(line.Substring(0, line.IndexOf(':')), ln);
                    line = line.Substring(line.IndexOf(':')+1);
                }
                line = line.Trim();
                if (line.StartsWith(".")) // an assembler command
                {
                    if (labelRun) continue;
                    string[] asm_cmd_token = line.Split(' ');
                    switch (asm_cmd_token[0])
                    {
                        case ".pos":
                            try
                            {
                                pos = int.Parse(asm_cmd_token[1]);
                                _binpos[ln] = pos;
                            }
                            catch (Exception)
                            {
                                throw new InvalidProgramException("Wrong assembler command at line " + ln);
                            }
                            break;
                        case ".align":
                            try
                            {
                                int pos_align = int.Parse(asm_cmd_token[1]);
                                if (pos % pos_align != 0)
                                    pos = pos / pos_align * pos_align + pos_align;
                            }
                            catch (Exception)
                            {
                                throw new InvalidProgramException("Wrong assembler command at line " + ln);
                            }
                            break;
                        case ".long":
                            mem.PutInt(pos, int.Parse(asm_cmd_token[1]));
                            pos += 4;
                            break;
                    }
                }
                else // ASM codes
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    string[] token = getTokens(line);
                    ICodeFunc cf = (ICodeFunc)Enum.Parse(typeof(ICodeFunc), token[0], true);

                    mem[pos] = (byte)cf;

                    if (!labelRun)
                    {
                        switch (Instruction.GetHigherType(cf))
                        {
                            case ICodeFunc.RRMOVL:
                            case ICodeFunc.OPL:
                                mem[pos + 1] = getRegisterByte(token[1], token[2]);
                                break;

                            case ICodeFunc.JXX:
                            case ICodeFunc.CALL:
                                int addr = getImm(token[1]);
                                if (_labels.Keys.Contains(token[1]))
                                    if (Properties.Settings.Default.JmpRelativePosition)
                                        addr = addr - pos - Instruction.GetInstrLength(cf);
                                mem.PutInt(pos + 1, addr);
                                break;

                            case ICodeFunc.IRMOVL:
                                mem[pos + 1] = getRegisterByte("", token[2]);
                                mem.PutInt(pos + 2, getImm(token[1]));
                                break;

                            case ICodeFunc.RMMOVL:
                                mem[pos + 1] = getRegisterByte(token[1], token[3]);
                                mem.PutInt(pos + 2, getImm(token[2]));
                                break;

                            case ICodeFunc.MRMOVL:
                                mem[pos + 1] = getRegisterByte(token[3], token[2]);
                                mem.PutInt(pos + 2, getImm(token[1]));
                                break;

                            case ICodeFunc.PUSHL:
                            case ICodeFunc.POPL:
                                mem[pos + 1] = getRegisterByte(token[1], "");
                                break;

                            default:
                                break;
                        }
                    }
                    pos += Instruction.GetInstrLength(cf);
                }
            }
            return pos;
        }

        public void Assemble(string outputFilename)
        {
            generateBin(true);
            int len = generateBin(false);

            // write to binary file
            var sw = System.IO.File.OpenWrite(outputFilename);
            sw.Write(mem.Buffer, 0, len);
            sw.Close();
        }

        private int getImm(string Imm)
        {
            int i = int.MinValue;
            if (Imm.StartsWith("$")) Imm = Imm.Substring(1);
            if (Imm.StartsWith("0x")) i = int.Parse(Imm.Substring(2), System.Globalization.NumberStyles.HexNumber);
            else if (!int.TryParse(Imm, out i)) if (_labels.Keys.Contains(Imm)) i = _binpos[_labels[Imm]] + Properties.Settings.Default.ProgramLoadMem;
            return i;
        }

        private byte getRegisterByte(string ra, string rb)
        {
            if (ra.StartsWith("%")) ra = ra.Substring(1);
            if (rb.StartsWith("%")) rb = rb.Substring(1);
            Register a, b;
            if (ra == "") a = Register.NONE; else a = (Register)Enum.Parse(typeof(Register), ra, true);
            if (rb == "") b = Register.NONE; else b = (Register)Enum.Parse(typeof(Register), rb, true);
            return (byte)(((int)a << 4) | (int)b);
        }

        private string[] getTokens(string line)
        {
            string[] tmp = new string[4];
            int last = -1, idx = 0; bool lastCharSpecial = false;
            for (int i = 0; i < line.Length; ++i)
            {
                if (line[i] == ' ' || line[i] == '(' || line[i] == ')' || line[i] == ',')
                {
                    if (!lastCharSpecial)
                    {
                        tmp[idx++] = line.Substring(last+1, i-last-1);
                    }
                    last = i;
                    lastCharSpecial = true;
                }
                else lastCharSpecial = false;
            }
            if (idx < 4) tmp[idx] = line.Substring(last + 1);
            return tmp;
        }

        public IEnumerable<Instruction> GetInstructions()
        {
            generateBin(true);
            int len = generateBin(false);
            for (int i = 0; i < len; )
            {
                Instruction ii;
                yield return ii = mem.GetInstruction(i);
                i += Instruction.GetInstrLength(ii.CodeFunc);
            }
        }
    }
}