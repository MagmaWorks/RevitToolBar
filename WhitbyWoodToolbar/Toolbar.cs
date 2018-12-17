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
using System.IO;
using System.Windows.Forms;

namespace WhitbyWoodToolbar
{
    public class Toolbar : IExternalApplication
    {
        UIControlledApplication revitApp;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            this.revitApp = application;

            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            //This could check for Dynamo packages on start up
            //CheckDynamo.checkDynamoPackages(); 

            
            application.CreateRibbonTab("Whitby Wood");

            Uri wwLogo = new Uri("pack://application:,,,/WhitbyWoodToolbar;component/resources/Link.png");
            var panel1 = application.CreateRibbonPanel("Whitby Wood", "General panel");
            panel1.AddItem(new PushButtonData(
                "WW Batch print",
                "Batch PDF",
                assemblyPath,
                "WhitbyWoodToolbar.WWBatchPrint")
            {
                ToolTip = "Batch print",
                LargeImage = new BitmapImage(wwLogo)
            });

            panel1.AddItem(new PushButtonData(
                "Check Dynamo version and packages",
                "Check Dynamo",
                assemblyPath,
                "WhitbyWoodToolbar.WWCheckDynamo")
            {
                ToolTip = "Batch print",
                LargeImage = new BitmapImage(wwLogo)
            });

            return Result.Succeeded;
        }


    }
}
