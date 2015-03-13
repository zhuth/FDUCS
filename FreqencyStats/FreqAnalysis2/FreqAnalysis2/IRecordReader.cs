using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreqAnalysis2
{
    interface IRecordReader
    {
        IEnumerable<Record> ReadRecords(string filename, Encoding enc, double sid);
    }
}
