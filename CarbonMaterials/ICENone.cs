using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ICENone : ICEMaterial
    {
        public override string Name => "None";

        public override double SequesteredCarbonDensity => 0;

        public override GWPMaterial getCopy()
        {
            var returnMaterial = new ICENone();
            returnMaterial.ConstructionFactor = this.ConstructionFactor;
            returnMaterial.DeConstructionFactor = this.DeConstructionFactor;
            returnMaterial.InUseFactor = this.InUseFactor;
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
            if (material is ICENone)
            {
                var m = material as ICENone;
                this.ConstructionFactor = m.ConstructionFactor;
                this.DeConstructionFactor = m.DeConstructionFactor;
                this.InUseFactor = m.InUseFactor;
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

        public ICENone()
        {
            Category = ICECategory.None;
            _massDensity = 0;
        }
    }
}
