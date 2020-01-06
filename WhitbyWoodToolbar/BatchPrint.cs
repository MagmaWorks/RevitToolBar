using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MagmaWorksToolbar
{
    [Transaction(TransactionMode.Manual)]
    public class BatchPrint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;

            List<Element> mySheets = new List<Element>();
            FilteredElementCollector sheets = new FilteredElementCollector(doc);
            mySheets.AddRange(sheets.OfClass(typeof(ViewSheet)).ToElements());
            
            foreach (var sheet in mySheets)
            {
                string sheetName = sheet.get_Parameter(BuiltInParameter.SHEET_NAME).AsString();
                string sheetNumber = sheet.get_Parameter(BuiltInParameter.SHEET_NUMBER).AsString();
                string currentRev = sheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION).AsString();
                string currentRevDate = sheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION_DATE).AsString();
                string currentRevDescript = sheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION_DESCRIPTION).AsString();
            }

            BatchPrintViewModel vm = new BatchPrintViewModel(commandData);
            BatchPrintControl userControl = new BatchPrintControl();
            Window batchWindow = new Window()
            {
                Content = userControl,
                DataContext = vm
            };

            batchWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}
