using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    public class ICEMaterialSteel : ICEMaterial
    {
        public override string Name
        {
            get
            {
                return _steelMaterial + " " + CarbonDensity;
            }
        }

        public override double CarbonDensity
        {
            get
            {
                return SteelCarbonData[_steelMaterial] * 7700;
            }
        }

        public List<string> Materials
        {
            get
            {
                return SteelMaterials;
            }
        }

        string _steelMaterial;

        public string SteelMaterial
        {
            get
            {
                return _steelMaterial;
            }
            set
            {
                _steelMaterial = value;
                RaisePropertyChanged(nameof(SteelMaterial));
                RaisePropertyChanged(nameof(CarbonDensity));
                Parent.ICEMaterialUpdated();
            }
        }

        public ICEMaterialSteel(string material, ElementCarbonVM parent)
        {
            _steelMaterial = material;
            Parent = parent;
            Category = ICECategory.Steel;
        }

        public ICEMaterialSteel(ElementCarbonVM parent)
        {
            Parent = parent;
        }

        public static Dictionary<string, double> SteelCarbonData = new Dictionary<string, double>
        {
            { "General Steel",1.46},{ "Bar & rod",1.4},{ "Coil (Sheet)",1.38},{ "Coil (Sheet) - Galvanised",1.54},{ "Pipe",1.45},{ "Plate",1.66},{ "Section",1.53}
        };

        public static List<string> SteelMaterials = new List<string>
        {
            "General Steel","Bar & rod","Coil (Sheet)","Coil (Sheet) - Galvanised","Pipe","Plate","Section" };
    }
}

