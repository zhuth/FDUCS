using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace y86sim
{
    public static class Utility
    {
        
        public static string ChangeExtName(string filename, string newExt)
        {
            int pdot = filename.LastIndexOf('.');
            if (pdot < 0) pdot = filename.Length;
            return filename.Substring(0, pdot) + "." + newExt;
        }

        public static string SafeSubstring(string from, int offset, int length = -1)
        {
            if (offset < 0) offset = 0;
            if (length < 0) return from.Substring(offset);
            return from.Substring(offset, length);
        }

        public static string SafeSubstringPos(string from, int start, int end)
        {
            if (end >= from.Length) end = from.Length - 1;
            if (start < 0) start = 0;
            return from.Substring(start, end - start + 1);
        }


        public static void PromptException(Exception ex)
        {
            string[] callers = ex.StackTrace.Split('\r','\n');
            string caller = callers[callers.Length - 1].Trim();
            Console.WriteLine("An error occurred: {1} ({0})", caller, ex.Message);
        }

        internal static bool CheckY86Class(Type type)
        {
            return (type.Assembly == typeof(Utility).Assembly) && !type.IsEnum;
        }
    }
}
