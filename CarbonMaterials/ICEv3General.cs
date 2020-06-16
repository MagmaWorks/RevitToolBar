using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ICEv3General : ICEMaterial
    {
        public override string Name => _material + ": " + _subMaterial + " " + A1toA3 + "kg/m3";

        [JsonProperty(PropertyName = "SequesteredCarbonDensity")]
        double _sequesteredCarbonDensity = 0;
        public override double SequesteredCarbonDensity => _sequesteredCarbonDensity;

        public override void copyMaterialFrom(GWPMaterial material)
        {
            throw new NotImplementedException();
        }

        public override GWPMaterial getCopy()
        {
            throw new NotImplementedException();
        }

        [JsonProperty(PropertyName = "EmbodiedCarbon")]
        double _embodiedCarbon;

        public override GWPValue A1toA3
        {
            get
            {
                if (IncludeSequesteredCarbon)
                {
                    return new GWPValue(_embodiedCarbon * MassDensity);
                }
                else
                {
                    return new GWPValue((_embodiedCarbon + _sequesteredCarbonDensity) * MassDensity);
                }
            }
        }

        public void UpdateValues(string material, string submaterial, double embodiedCarbon, double sequesteredCarbon, double massDensity, string guid)
        {
            _material = material;
            _subMaterial = submaterial;
            _embodiedCarbon = embodiedCarbon;
            _sequesteredCarbonDensity = sequesteredCarbon;
            _massDensity = massDensity;
            _guid = guid;
        }

        public override double MassDensity { get => _massDensity; protected set => _massDensity = value; }

        [JsonProperty(PropertyName = "GUID")]
        string _guid;
        public string Guid { get => _guid; }
        [JsonProperty(PropertyName = "Material")]
        string _material;
        [JsonProperty(PropertyName = "SubMaterial")]
        string _subMaterial;

        public ICEv3General(string material, string submaterial, double embodiedCarbon, double sequesteredCarbon, double massDensity, string guid)
        {
            _material = material;
            _subMaterial = submaterial;
            _embodiedCarbon = embodiedCarbon;
            _sequesteredCarbonDensity = sequesteredCarbon;
            _massDensity = massDensity;
            _guid = guid;
            Category = ICECategory.GeneralV3;
        }

        public ICEv3General()
        {
            _material = "AggregateSand";
            _subMaterial = "AggregateSand, General aggregate and sand";
            _embodiedCarbon = 0;
            _sequesteredCarbonDensity = 0;
            _massDensity = 0;
            Category = ICECategory.GeneralV3;
        }

        public static List<ICEv3DBMaterial> ReadICEv3Materials()
        {
            byte[] jsonToRead = Resource1.ICEv3;
            StreamReader reader = new StreamReader(new MemoryStream(jsonToRead));
            JsonSerializer serialiser = new JsonSerializer();
            var test = ((List<ICEv3DBMaterial>)serialiser.Deserialize(reader, typeof(List<ICEv3DBMaterial>)));
            return test;
        }

        public class ICEv3DBMaterial
        {
            public string GUID { get; set; }
            public string Material { get; set; }
            public string SubMaterial { get; set; }
            public string ICEv3DBName { get; set; }
            public string Comments { get; set; }
            public string Boundaries { get; set; }
            public double MaterialDensity { get; set; }
            public string IsSequestrationIncluded { get; set; }
            public double SequesteredCarbon { get; set; }
            public double EmbodiedCarbonPerMass { get; set; }
        }
    }
}
