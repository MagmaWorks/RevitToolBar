using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonMaterials
{
    public class GWPMaterialNone : GWPMaterial
    {
        public override string Name => "None";

        public override double SequesteredCarbonDensity => 0;

        public override GWPMaterialType GWPMaterialType { get; } = GWPMaterialType.None;

        public GWPMaterialNone()
        {
        }

        public override GWPMaterial getCopy()
        {
            throw new NotImplementedException();
        }

        public override void copyMaterialFrom(GWPMaterial material)
        {
            throw new NotImplementedException();
        }
    }
}
