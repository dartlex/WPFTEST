using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
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
            string tabName = "MisterPlugin2.0";
            application.CreateRibbonTab(tabName);
            RibbonPanel panel = application.CreateRibbonPanel(tabName,"Главное меню");
            PushButtonData buttonData = new PushButtonData("MyFirstButton", "Запустить\nплагин", typeof(App).Assembly.Location, "MisterPlugin2._0.StartCommand");

            string assemblyName = typeof(App).Assembly.GetName().Name;
            string resourcePath = $"pack://application:,,,/{assemblyName};component/test.png";
            buttonData.LargeImage = new BitmapImage(new Uri(resourcePath));

            panel.AddItem(buttonData);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        
    }
} 


