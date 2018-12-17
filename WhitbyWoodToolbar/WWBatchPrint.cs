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

namespace WhitbyWoodToolbar
{
    [Transaction(TransactionMode.Manual)]
    public class WWBatchPrint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
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
