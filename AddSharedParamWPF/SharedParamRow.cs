using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AddSharedParamWPF
{
    public class SharedParamRow
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }

        public ExternalDefinition Definition { get; set; }

    }
}
