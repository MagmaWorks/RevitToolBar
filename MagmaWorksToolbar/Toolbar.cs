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

namespace MagmaWorksToolbar
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

            application.CreateRibbonTab("Magma Works");

            Uri batchIcon = new Uri("pack://application:,,,/MagmaWorksToolbar;component/resources/dwgpdf.png");
            var panel1 = application.CreateRibbonPanel("Magma Works", "General panel");
            panel1.AddItem(new PushButtonData(
                "Batch print",
                "Batch PDF",
                assemblyPath,
                "MagmaWorksToolbar.BatchPrint")
            {
                ToolTip = "Batch print",
                LargeImage = new BitmapImage(batchIcon)
            });

            checkDynamoFolder(@"W:\WW General\WW Software Resources\Dynamo\Packages");

            //Uri dynamoCheckIcon = new Uri("pack://application:,,,/WhitbyWoodToolbar;component/resources/DynamoCheck.png");
            //panel1.AddItem(new PushButtonData(
            //    "Check Dynamo version and packages",
            //    "Check Dynamo",
            //    assemblyPath,
            //    "WhitbyWoodToolbar.WWCheckDynamo")
            //{
            //    ToolTip = "Batch print",
            //    LargeImage = new BitmapImage(dynamoCheckIcon)
            //});

            Uri carbonIcon = new Uri("pack://application:,,,/MagmaWorksToolbar;component/resources/CarbonCalculator.png");
            panel1.AddItem(new PushButtonData(
                "Carbon Counter",
                "Carbon Counter",
                assemblyPath,
                "MagmaWorksToolbar.CarbonCounter")
            {
                ToolTip = "Carbon Counter",
                LargeImage = new BitmapImage(carbonIcon)
            });

            return Result.Succeeded;
        }

        void checkDynamoFolder(string location)
        {
            string localDynamoPackagesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Dynamo\Dynamo Revit";
            var localVersionFolders = Directory.GetDirectories(localDynamoPackagesFolder);
            string[] localVersionFolderNames = new string[localVersionFolders.Count()];
            for (int i = 0; i < localVersionFolders.Length; i++)
            {
                localVersionFolderNames[i] = Path.GetFileName(localVersionFolders[i]);
            }

            var versionFolders = Directory.GetDirectories(location);
            foreach (var versionFolder in versionFolders)
            {
                string versionToCheck = Path.GetFileName(versionFolder);
                if (!localVersionFolderNames.Contains(versionToCheck)) ;
                { MessageBox.Show("Dynamo pacakges folder " + versionFolder + " is missing." + versionToCheck + localVersionFolderNames[0]); }

                var packageFolders = Directory.GetDirectories(versionFolder);
                foreach (var packageFolder in packageFolders)
                {
                    if (File.Exists(packageFolder + @"\pkg.json"))
                    {
                        var jsonData = System.IO.File.ReadAllText(packageFolder + @"\pkg.json");
                        DynamoPackageInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<DynamoPackageInfo>(jsonData);
                        MessageBox.Show(info.name + " " + info.version);
                    }
                }                
            }
        }

    }

    public class DynamoPackageInfo
    {
        public string name { get; set; }
        public string version { get; set; }
    }
}
