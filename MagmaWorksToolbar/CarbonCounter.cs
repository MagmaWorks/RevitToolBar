using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagmaWorksToolbar
{
    [Transaction(TransactionMode.Manual)]
    public class CarbonCounter : IExternalCommand
    {
        const double _inchesToMm = 25.4;
        const double _footToMm = 12 * _inchesToMm;
        const double _footToM = _footToMm / 1000;
        const double _cubicFtToM = _footToM * _footToM * _footToM;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            List<string> materialsUsed = new List<string>();
            MaterialsVM myVM = new MaterialsVM();

            var doc = commandData.Application.ActiveUIDocument.Document;
            var cats = new List<BuiltInCategory>{
                BuiltInCategory.OST_StructuralFoundation,
                BuiltInCategory.OST_StructuralFraming,
                BuiltInCategory.OST_StructuralColumns,
                BuiltInCategory.OST_Floors,
                BuiltInCategory.OST_EdgeSlab,
                BuiltInCategory.OST_Walls
            };


            //enable volume computations
            using (Transaction t = new Transaction(doc, "Turn on vol calc"))
            {
                t.Start();
                var settings = AreaVolumeSettings.GetAreaVolumeSettings(doc);
                settings.ComputeVolumes = true;
                t.Commit();
            }

            foreach (var cat in cats)
            {
                var catFilters = new List<ElementFilter> { new ElementCategoryFilter(cat) };

                var filter = new LogicalOrFilter(catFilters);

                var structures = new FilteredElementCollector(doc)
                    .WherePasses(filter)
                    .ToElements();

                double vol = 0;
                int counter = 0;

                foreach (var item in structures)
                {
                    var mat = item.LookupParameter("Structural Material");
                    if (mat == null)
                    {
                        var itemType = doc.GetElement(item.GetTypeId());
                        if (itemType != null)
                        {
                            mat = itemType.LookupParameter("Structural Material");
                        }
                    }
                    var volParam = item.LookupParameter("Volume");

                    var elemType = item.GetTypeId();
                    string name = "";
                    var elemTy = doc.GetElement(elemType) as ElementType;
                    if (elemTy != null)
                    {
                        name = elemTy.FamilyName + ": " + elemTy.Name;
                    }

                    string matName = "";
                    if (volParam != null)
                    {
                        if (mat != null)
                        {
                            matName = mat.AsValueString();
                            if (!materialsUsed.Contains(matName))
                            {
                                materialsUsed.Add(matName);
                            }
                        }

                        var test = volParam.AsValueString();
                        double metricVol = volParam.AsDouble() * _cubicFtToM;
                        vol += metricVol;
                        counter++;

                        myVM.AddElement(name, metricVol, matName, doc, cat);
                    }
                }
            }

            var userControl = new UserControl1();
            Window carbonWindow = new Window()
            {
                Content = userControl,
                DataContext = myVM
            };

            carbonWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}
