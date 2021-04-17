using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;


namespace ViewTemplateUtils 
{
    public class MyDocument
    {
        public Document doc { get; }
        public MyDocument(Document Doc)
        {
            doc = Doc;
        }

        public override string ToString()
        {
            string title = doc.Title;
            return title;
        }
    }
}
