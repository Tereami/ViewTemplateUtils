using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB; //для работы с элементами модели Revit
using Autodesk.Revit.UI; //для работы с элементами интерфейса
using Autodesk.Revit.UI.Selection; //работы с выделенными элементами
using Autodesk.Revit.ApplicationServices;


namespace ViewTemplateUtils
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCopyTemplate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application app = commandData.Application.Application;
            Document mainDoc = commandData.Application.ActiveUIDocument.Document;

            DocumentSet docSet = app.Documents;
            List<MyDocument> allDocs = new List<MyDocument>();
            foreach(Document doc in app.Documents)
            {
                if (doc.Title == mainDoc.Title) continue;
                if (doc.IsValidObject)
                {
                    MyDocument myDoc = new MyDocument(doc);
                    allDocs.Add(myDoc);
                }
            }

            if(allDocs.Count ==0)
            {
                message = "Нет открытых документов для копирования!";
                return Result.Failed;
            }

            FormSelectDocument form1 = new FormSelectDocument(allDocs);
            form1.ShowDialog();
            if (form1.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

            Document selectedDoc = form1.selectedDocument.doc;

            List<View> templates = new FilteredElementCollector(selectedDoc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate == true)
                .ToList();
            List<MyView> myViews = templates
                .OrderBy(i => i.Name) //ViewName)
                .Select(i => new MyView(i))
                .ToList();


            FormSelectTemplates form2 = new FormSelectTemplates(myViews);
            form2.ShowDialog();
            if (form2.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

            List<ElementId> templateIds = form2.selectedTemplates.Select(i => i.view.Id).ToList();
            CopyPasteOptions cpo = new CopyPasteOptions();
            cpo.SetDuplicateTypeNamesHandler(new DuplicateNamesHandler());

            using (Transaction t = new Transaction(mainDoc))
            {
                t.Start("Копирование шаблонов видов");

                
                ElementTransformUtils.CopyElements(selectedDoc, templateIds, mainDoc, Transform.Identity, cpo);
                        
                t.Commit();
            }

            string msg = "Успешно скопировано шаблонов: " + templateIds.Count.ToString();
            if (DuplicateTypes.types.Count > 0) msg += "\nПродублированы: " + DuplicateTypes.ReturnAsString();

            TaskDialog.Show("Отчет", msg);

            return Result.Succeeded;
        }
    }
}
