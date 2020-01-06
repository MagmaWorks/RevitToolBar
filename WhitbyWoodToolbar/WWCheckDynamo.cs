using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    [Transaction(TransactionMode.Manual)]
    public class WWCheckDynamo : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            CheckDynamo.checkDynamoPackages();

            return Result.Succeeded;
        }
    }
}
