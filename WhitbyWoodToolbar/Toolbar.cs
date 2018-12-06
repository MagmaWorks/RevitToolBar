using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace WhitbyWoodToolbar
{
    public class Toolbar : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            
            application.CreateRibbonTab("Whitby Wood");

            Uri wwLogo = new Uri("pack://application:,,,/WhitbyWoodToolbar;component/resources/Link.png");
            var panel1 = application.CreateRibbonPanel("Whitby Wood", "General panel");
            panel1.AddItem(new PushButtonData(
                "WW Standards",
                "WW",
                assemblyPath,
                "WhitbyWoodToolbar.WWStandards")
            {
                ToolTip = "Link to WW BIM standards",
                LargeImage = new BitmapImage(wwLogo)
            });

            return Result.Succeeded;
        }
    }
}
