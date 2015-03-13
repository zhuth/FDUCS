using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FreqAnalysis2
{
    class SCReader : IRecordReader
    {
        
        public IEnumerable<Record> ReadRecords(string filename, Encoding enc, double sid)
        {
            Regex regDelete = new Regex(@"(\s|【.*】|［.*］|（.*）|[0-9a-z])", RegexOptions.Compiled);
            bool finished = false;
            Record rec = new Record(); rec.sourceId = sid;
            foreach (string line in File.ReadAllLines(filename, enc))
            {
                string tmp = regDelete.Replace(line, "").Trim();
                if (tmp.Trim() == "")
                {
                    if (!finished)
                    {
                        finished = true;
                        if (rec.text != "") yield return rec;
                        rec.text = "";
                    }
                    continue;
                }
                else
                {
                    if (finished)
                    {
                        finished = false;
                        rec.title = tmp;
                    }
                    else
                        rec.text += tmp;
                }
            }
        }
    }
}
