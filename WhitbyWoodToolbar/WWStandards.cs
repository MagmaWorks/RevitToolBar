using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Linq;

namespace WhitbyWoodToolbar
{
    [Transaction(TransactionMode.Manual)]
    public class WWStandards : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document, "WW_PDF");
            trans.Start();
            MessageBox.Show(
                "Batch printing..." + Environment.NewLine +
                "Using Bluebeam PDF printer and" + Environment.NewLine +
                "Bluebeam A1L print settings"
                );

            var doc = commandData.Application.ActiveUIDocument.Document;
            string filePath = @"C:\Users\Alex Baalham\Documents\";

            var printM = commandData.Application.ActiveUIDocument.Document.PrintManager;
            printM.SelectNewPrintDriver("Bluebeam PDF");

            var ps = new FilteredElementCollector(doc);
            List<Element> myPrintSettings = ps.OfClass(typeof(PrintSetting)).ToList();
            foreach (var setting in myPrintSettings)
            {
                if (setting.Name == "Bluebeam A1L")
                {
                    printM.PrintSetup.CurrentPrintSetting = setting as PrintSetting;
                    printM.Apply();
                }
            }

            var settings = printM.PrintSetup.CurrentPrintSetting;
            printM.PrintToFile = true;
            //PaperSize A1PaperSize = null;

            
            //foreach (PaperSize item in printM.PaperSizes)
            //{
            //    if (item.Name.Contains("ISO_A1"))
            //    {
            //        A1PaperSize = item;
            //        settings.PrintParameters.PaperSize = item ;
            //    }
            //}

            //commandData.Application.ActiveUIDocument.Document.
            List<Element> mySheets = new List<Element>();
            FilteredElementCollector sheets = new FilteredElementCollector(doc);
            mySheets.AddRange(sheets.OfClass(typeof(ViewSheet)).ToElements());
            string output = "Sheets printed: " + Environment.NewLine;

            foreach (var sheet in mySheets)
            {
                output += sheet.get_Parameter(BuiltInParameter.SHEET_NUMBER).AsString() + " " + sheet.Name + Environment.NewLine;
                string sheetNumber = sheet.LookupParameter("Sheet Number").AsString();
                printM.PrintToFileName = @"C:\Users\Alex Baalham\Documents\" + sheetNumber + @".pdf";
                printM.SubmitPrint(sheet as ViewSheet);
            }
            MessageBox.Show(output);
            trans.Commit();
            return Result.Succeeded;
        }
    }
}
