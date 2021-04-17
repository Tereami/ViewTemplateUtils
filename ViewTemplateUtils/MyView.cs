using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;


namespace ViewTemplateUtils
{
    public class MyView
    {
        public View view { get; }

        public MyView(View v)
        {
            view = v;
        }
        public override string ToString()
        {
            string name = view.Name; //view.ViewName;
            return name;
        }
    }
}
