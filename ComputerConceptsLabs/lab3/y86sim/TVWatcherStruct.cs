using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim
{
    class TVWatcherStruct
    {
        public string tvpath;
        public Type type;
        public object value;
        
        public TVWatcherStruct(string tvp, Type typ, string val) {
            tvpath = tvp;
            type = typ;
            value = val;
        }
    }
}
