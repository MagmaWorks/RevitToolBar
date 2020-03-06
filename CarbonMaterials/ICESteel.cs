using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ICESteel : ICEMaterial
    {
        public override string Name
        {
            get
            {
                return _steelMaterial + " " + string.Format("{0:0.00}", TotalAtoC) + "kg CO2e/m3"; 
            }
        }

        public override double SequesteredCarbonDensity => 0;

        public override GWPValue A1toA3
        {
            get
            {
                return new GWPValue(SteelCarbonData[_steelMaterial] * MassDensity);
            }
        }

        public List<string> SteelMaterials
        {
            get
            {
                return SteelMaterialsData;
            }
        }

        [JsonProperty(PropertyName ="SteelMaterial")]
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
            }
        }

        public ICESteel(string material)
        {
            _steelMaterial = material;
            Category = ICECategory.Steel;
            _massDensity = 7700;
        }

        public ICESteel()
        {
            _steelMaterial = "General Steel";
            Category = ICECategory.Steel;
            _massDensity = 7700;
        }

        public static Dictionary<string, double> SteelCarbonData = new Dictionary<string, double>
        {
            { "General Steel",1.46},{ "Bar & rod",1.4},{ "Coil (Sheet)",1.38},{ "Coil (Sheet) - Galvanised",1.54},{ "Pipe",1.45},{ "Plate",1.66},{ "Section",1.53}
        };

        public static List<string> SteelMaterialsData = new List<string>
        {
            "General Steel","Bar & rod","Coil (Sheet)","Coil (Sheet) - Galvanised","Pipe","Plate","Section" };

        public override GWPMaterial getCopy()
        {
            var returnMaterial = new ICESteel(this.SteelMaterial);
            returnMaterial.ConstructionFactor = this.ConstructionFactor;
            returnMaterial.DeConstructionFactor = this.DeConstructionFactor;
            returnMaterial.InUseFactor = this.InUseFactor;
            returnMaterial._massDensity = this.MassDensity;
            returnMaterial.IncludeSequesteredCarbon = this.IncludeSequesteredCarbon;
            
            foreach (var item in this.TransportsToSite)
            {
                returnMaterial.TransportsToSite.Add(item);
            }
            foreach (var item in this.TransportsToDispoal)
            {
                returnMaterial.TransportsToDispoal.Add(item);
            }
            return returnMaterial;
        }

        public override void copyMaterialFrom(GWPMaterial material)
        {
            if (material is ICESteel)
            {
                var m = material as ICESteel;
                this.SteelMaterial = m.SteelMaterial;
                this.ConstructionFactor = m.ConstructionFactor;
                this.DeConstructionFactor = m.DeConstructionFactor;
                this.InUseFactor = m.InUseFactor;
                this._massDensity = m.MassDensity;
                this.IncludeSequesteredCarbon = m.IncludeSequesteredCarbon;
                this.TransportsToSite.Clear();
                this.TransportsToDispoal.Clear();

                foreach (var item in m.TransportsToSite)
                {
                    this.TransportsToSite.Add(item);
                }
                foreach (var item in m.TransportsToDispoal)
                {
                    this.TransportsToDispoal.Add(item);
                }
            }
        }
    }
}
