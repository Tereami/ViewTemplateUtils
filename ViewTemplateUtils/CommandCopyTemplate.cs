#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
#endregion

namespace ViewTemplateUtils
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCopyTemplate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new RbsLogger.Logger("CopyTemplates"));

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
            Debug.Write("Docs count: " + allDocs.Count);
            if(allDocs.Count == 0)
            {
                message = "Нет открытых документов для копирования!";
                return Result.Failed;
            }

            FormSelectDocument form1 = new FormSelectDocument(allDocs);
            form1.ShowDialog();
            if (form1.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            Document selectedDoc = form1.selectedDocument.doc;
            Debug.WriteLine("Selected doc: " + selectedDoc.Title);

            List<View> templates = new FilteredElementCollector(selectedDoc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate == true)
                .ToList();
            Debug.WriteLine("Templates found: " + templates.Count);
            List<MyView> myViews = templates
                .OrderBy(i => i.Name) 
                .Select(i => new MyView(i))
                .ToList();

            FormSelectTemplates form2 = new FormSelectTemplates(myViews);
            form2.ShowDialog();
            if (form2.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            List<ElementId> templateIds = form2.selectedTemplates.Select(i => i.view.Id).ToList();
            Debug.WriteLine("Selected templates: " + templateIds.Count);
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

            Debug.WriteLine(msg);
            TaskDialog.Show("Отчет", msg);

            return Result.Succeeded;
        }
    }
}
