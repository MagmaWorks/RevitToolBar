using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class GWPMaterial
    {
        public virtual GWPValue A1 { get;  } = new GWPValue();
        public virtual GWPValue A2 { get;  } = new GWPValue();
        public virtual GWPValue A3 { get;  } = new GWPValue();
        public virtual GWPValue A4 { get;  } = new GWPValue();
        public virtual GWPValue A5 { get;  } = new GWPValue();
        public virtual GWPValue A1toA3 { get;  } = new GWPValue();
        public virtual GWPValue B1 { get;  } = new GWPValue();
        public virtual GWPValue B2 { get;  } = new GWPValue();
        public virtual GWPValue B3 { get;  } = new GWPValue();
        public virtual GWPValue B4 { get;  } = new GWPValue();
        public virtual GWPValue B5 { get;  } = new GWPValue();
        public virtual GWPValue B6 { get;  } = new GWPValue();
        public virtual GWPValue B7 { get;  } = new GWPValue();
        public virtual GWPValue C1 { get;  } = new GWPValue();
        public virtual GWPValue C2 { get;  } = new GWPValue();
        public virtual GWPValue C3 { get;  } = new GWPValue();
        public virtual GWPValue C4 { get;  } = new GWPValue();

        public double TotalA => A1toA3.Value + A4.Value + A5.Value;
        public double TotalB => B1.Value + B2.Value + B3.Value + B4.Value + B5.Value + B6.Value + B7.Value;
        public double TotalC => C1.Value + C2.Value + C3.Value + C4.Value;
        public double TotalAtoC => TotalA + TotalB + TotalC;

        public abstract string Name { get; }
        

        public virtual double MassDensity { get; protected set; }
        public abstract double SequesteredCarbonDensity { get; }
        public abstract GWPMaterialType GWPMaterialType { get; }
        public abstract GWPMaterial getCopy();
        public abstract void copyMaterialFrom(GWPMaterial material);
        [JsonProperty]
        public bool IncludeSequesteredCarbon { get; set; } = true;



    }   
}
