using System;
using System.Collections.Generic;
using System.Text;

namespace NapalmINI
{
    public class INIComment
    {
        public int Line;
        public string Value;

        public INIComment(int line, string value)
        {
            this.Line = line;
            this.Value = value;
        }

        public override string ToString() { return "{ LINE:" + Line + " VALUE:" + Value + " }"; }
    }
}
