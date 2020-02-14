using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ICEMaterial : GWPMaterial
    {
        [JsonProperty]
        public ICECategory Category { get; protected set; }

        public override GWPMaterialType GWPMaterialType => GWPMaterialType.ICE;

        public static List<string> MaterialTypes = new List<string> { "None", "Concrete", "Steel", "Timber" };

        List<MaterialTransport> _transportsToSite = new List<MaterialTransport>();
        public List<MaterialTransport> TransportsToSite
        {
            get
            {
                return _transportsToSite;
            }
        }

        [JsonProperty]
        List<MaterialTransport> _transportsToDisposal = new List<MaterialTransport>();
        public List<MaterialTransport> TransportsToDispoal
        {
            get
            {
                return _transportsToDisposal;
            }
        }

        [JsonProperty]
        double _constructionFactor = 0;
        public double ConstructionFactor
        {
            get
            {
                return _constructionFactor;
            }
            set
            {
                _constructionFactor = value;
            }
        }

        public override GWPValue A4
        {
            get
            {
                return new GWPValue(_transportsToSite.Sum(a => a.CarbonPerKG * MassDensity));
            }
        }

        public override GWPValue A5
        {
            get
            {
                return new GWPValue(ConstructionFactor);
            }
        }

        [JsonProperty]
        double _inUseFactor = 0;
        public double InUseFactor
        {
            get
            {
                return _inUseFactor;
            }
            set
            {
                _inUseFactor = value;
            }
        }

        public override GWPValue B1
        {
            get
            {
                return new GWPValue(_inUseFactor);
            }
        }

        [JsonProperty]
        double _deConstructionFactor = 0;
        public double DeConstructionFactor
        {
            get
            {
                return _deConstructionFactor;
            }
            set
            {
                _deConstructionFactor = value;
            }
        }

        public override GWPValue C1
        {
            get
            {
                return new GWPValue(DeConstructionFactor);
            }
        }

        public override GWPValue C2
        {
            get
            {
                return new GWPValue(_transportsToDisposal.Sum(a => a.CarbonPerKG * MassDensity));

            }
        }

        [JsonProperty]
        double _recyclingReuseFactor = 0;
        public double RecyclingReuseFactor
        {
            get
            {
                return _recyclingReuseFactor;
            }
            set
            {
                _recyclingReuseFactor = value;
            }
        }

        public override GWPValue C3
        {
            get
            {
                return new GWPValue(RecyclingReuseFactor);
            }
        }

        [JsonProperty]
        GWPValue _c4Value;
        public override GWPValue C4 => (_c4Value = new GWPValue(0.013 * MassDensity)); // RICS recommend 0.013 default

        [JsonProperty]
        protected double _massDensity;
        public override double MassDensity
        {
            get
            {
                return _massDensity;
            }
        }

        public void ChangeMassDensity(double density)
        {
            _massDensity = density;
        }
    }
}