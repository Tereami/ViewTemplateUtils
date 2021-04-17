using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ViewTemplateUtils
{
    class DuplicateNamesHandler : IDuplicateTypeNamesHandler
    {
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            Document doc = args.Document;
            List<ElementId> ids = args.GetTypeIds().ToList();
            foreach(ElementId id in ids)
            {
                Element elem = doc.GetElement(id);
                if (elem is View)
                {
                    DuplicateTypes.types.Add(elem.Name);
                }
            }
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }
}
