using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DuplicateElements
{
    public class DuplicateElements : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            var col = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .WhereElementIsNotElementType();
            var groupedElements = col
                .Cast<FamilyInstance>()
                .Where(point => point.Location is LocationPoint)
                .GroupBy(fi =>
                {
                    LocationPoint lp = fi.Location as LocationPoint;
                    XYZ point = lp.Point;

                    int x = (int)Math.Round(UnitUtils.ConvertFromInternalUnits(point.X, UnitTypeId.Millimeters));
                    int y = (int)Math.Round(UnitUtils.ConvertFromInternalUnits(point.Y, UnitTypeId.Millimeters));
                    int z = (int)Math.Round(UnitUtils.ConvertFromInternalUnits(point.Z, UnitTypeId.Millimeters));

                    return $"{x}_{y}_{z}";
                });
            var duplicateElements = new List<ElementId>();
            return Result.Succeeded;
        }
    }
}
