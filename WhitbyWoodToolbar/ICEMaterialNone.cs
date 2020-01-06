using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    public class ICEMaterialNone : ICEMaterial
    {
        public override string Name => "No material assigned";

        public override double CarbonDensity => 0;

        public ICEMaterialNone(ElementCarbonVM parent)
        {
            Category = ICECategory.None;
            Parent = parent;
        }
    }
}
