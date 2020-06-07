using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ICEGeneral : ICEMaterial
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
                    return new GWPValue(_embodiedCarbon);
                }
                else
                {
                    return new GWPValue(_embodiedCarbon + _sequesteredCarbonDensity);
                }
            }
        }

        public override double MassDensity { get => _massDensity; protected set => _massDensity = value; }

        [JsonProperty(PropertyName = "Material")]
        string _material;
        [JsonProperty(PropertyName = "SubMaterial")]
        string _subMaterial;

        public ICEGeneral(string material, string submaterial, double embodiedCarbon, double sequesteredCarbon)
        {
            _material = material;
            _subMaterial = submaterial;
            _embodiedCarbon = embodiedCarbon;
            _sequesteredCarbonDensity = sequesteredCarbon;
        }

        Dictionary<string,ICEv3DBMaterial> readICEv3Materials()
        {
            byte[] jsonToRead = Resource1.ICEv3;
            StreamReader reader = new StreamReader(new MemoryStream(jsonToRead));
            JsonSerializer serialiser = new JsonSerializer();
            var test = ((List<ICEv3DBMaterial>)serialiser.Deserialize(reader, typeof(List<ICEv3DBMaterial>)));
            return test.ToDictionary(a => a.GUID);
        }

        private class ICEv3DBMaterial
        {
            public string GUID { get; private set; }
            public string Material { get; private set; }
            public string SubMaterial { get; private set; }
            public string ICEv3DBName { get; private set; }
            public string Comments { get; private set; }
            public string Boundaries { get; private set; }
            public double MaterialDensity { get; private set; }
            public string IsSequestrationIncluded { get; private set; }
            public double SequesteredCarbon { get; private set; }
            public double EmbodiedCarbonPerMass { get; private set; }
        }
    }
}
