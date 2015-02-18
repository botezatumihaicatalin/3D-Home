using System;

namespace Home3d.Model
{
    public class ParseException : Exception
    {
        public string Line { get; set; }
        public string Reason { get; set; }

        public ParseException(string line, string reason)
            : base(string.Format("Error when parsing the line '{0}' : {1}", line, reason))
        {
            
        }
    }
}
