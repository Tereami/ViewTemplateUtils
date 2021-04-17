using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewTemplateUtils
{
    public static class DuplicateTypes
    {
        public static List<string> types = new List<string>();

        public static string ReturnAsString()
        {
            string msg = "";
            for(int i = 0; i< types.Count; i++)
            {
                msg += types[i];
                if (i != types.Count - 1) msg += ";\n";
            }
            return msg;
        }
    }
}
