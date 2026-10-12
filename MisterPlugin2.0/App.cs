using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MisterPlugin2._0
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string name = "MisterPlugin2.0";
            application.CreateRibbonTab(name);
            RibbonPanel panel = application.CreateRibbonPanel(name,"Главное меню");
            PushButtonData buttonData = new PushButtonData("MyFirstButton", "Запустить\nплагин", typeof(App).Assembly.Location, "MyFirstRevitPlugin.Command");
            panel.AddItem(buttonData);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        
    }
}
