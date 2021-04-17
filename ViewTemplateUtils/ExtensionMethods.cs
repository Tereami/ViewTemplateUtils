using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ViewTemplateUtils
{
    public static class ExtensionMethods
    {
        public static string ToString(this Document doc)
        {
            string title = doc.Title;
            return title;
        }
    }
}
