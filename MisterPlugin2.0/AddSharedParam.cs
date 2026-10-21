using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddSharedParamWPF;

namespace MisterPlugin2._0
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AddSharedParam : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (!doc.IsFamilyDocument)
            {
                TaskDialog.Show("Ошибка", "Данный инструмент можно открыть только в редакторе семейства");
                return Result.Cancelled;
            }
            SharedParamFamily window = new SharedParamFamily(doc);

            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}
