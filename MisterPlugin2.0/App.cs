using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Reflection;
using System.IO;

namespace MisterPlugin2._0
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "MisterPlugin2.0";

            var assemblyPath = Assembly.GetExecutingAssembly();

            var assemblyDir = new FileInfo(assemblyPath.Location).DirectoryName;
            application.CreateRibbonTab(tabName);

            RibbonPanel panel = application.CreateRibbonPanel(tabName,"Главное меню");

            PushButtonData buttonData = new PushButtonData("MyFirstButton", "Запустить\nплагин", assemblyPath.Location, "MisterPlugin2._0.StartCommand");

            var imagePath = Path.Combine(assemblyDir, @"Images\test.png");
            var ico = new BitmapImage(new Uri(imagePath));
            buttonData.LargeImage = ico;

            panel.AddItem(buttonData);

            PushButtonData buttonData2 = new PushButtonData("AddParamsBtn", "Добавление \nпараметров", assemblyPath.Location, "MisterPlugin2._0.AddSharedParam");
            imagePath = Path.Combine(assemblyDir, @"Images\add.png");
            ico = new BitmapImage(new Uri(imagePath));
            buttonData2.LargeImage = ico;
            panel.AddItem(buttonData2);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        
    }
} 


