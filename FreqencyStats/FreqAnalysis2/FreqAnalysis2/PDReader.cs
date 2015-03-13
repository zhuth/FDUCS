using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FreqAnalysis2
{
    class PDReader : IRecordReader
    {
        public IEnumerable<Record> ReadRecords(string filename, Encoding enc, double sid)
        {
            bool finished = true;
            Record rec = new Record(); rec.sourceId = sid;        
            foreach (string line in File.ReadAllLines(filename, enc))
            {
                string tmp = line.Trim();
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
                        rec.title = tmp.Substring(0, tmp.IndexOf(' '));
                        rec.sourceId = double.Parse(rec.title.Split('-')[1]);
                    }
                    else
                        rec.text += tmp.Substring(tmp.IndexOf(' ') + 1);
                }
            }
        }
    }
}
