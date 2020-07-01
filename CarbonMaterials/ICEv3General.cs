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
                    return new GWPValue((_embodiedCarbon - _sequesteredCarbonDensity) * MassDensity);
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

        public override GWPValue C3
        {
            get
            {
                if (IncludeSequesteredCarbon)
                    return new GWPValue(RecyclingReuseFactor - _sequesteredCarbonDensity * MassDensity);
                else
                    return new GWPValue(RecyclingReuseFactor);
            }
        }

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
            _embodiedCarbon = 0.0075;
            _sequesteredCarbonDensity = 0;
            _massDensity = 1600;
            _guid = "52bdfeb3-979f-4fcd-b4eb-6ffbda643c52";
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

        public static ICEv3General GetBlockwork100mm()
        {
            var dBase = ReadICEv3Materials().Where(a => a.GUID == "37da3732-82a2-4e71-8e6d-3842e99103f0").First();
            var returnMat = new ICEv3General(dBase.Material, dBase.SubMaterial, dBase.EmbodiedCarbonPerMass, dBase.SequesteredCarbon, dBase.MaterialDensity * 0.1, dBase.GUID);
            var siteTrans = MaterialTransport.DefaultDieselRigidHGV();
            siteTrans.Distance = 20;
            returnMat.TransportsToSite.Add(siteTrans);
            var dispTrans = MaterialTransport.DefaultDieselRigidHGV();
            dispTrans.Distance = 50;
            returnMat.TransportsToSite.Add(dispTrans);

            return returnMat;
        }

        public static ICEv3General GetSteelStud20kg()
        {
            var dBase = ReadICEv3Materials().Where(a => a.GUID == "6db285f8-2ce3-4414-93b0-4bbd893bef76").First();
            var returnMat = new ICEv3General(dBase.Material, dBase.SubMaterial, dBase.EmbodiedCarbonPerMass, dBase.SequesteredCarbon, 5, dBase.GUID);
            var siteTrans = MaterialTransport.DefaultDieselRigidHGV();
            siteTrans.Distance = 20;
            returnMat.TransportsToSite.Add(siteTrans);
            var dispTrans = MaterialTransport.DefaultDieselRigidHGV();
            dispTrans.Distance = 50;
            returnMat.TransportsToSite.Add(dispTrans);

            return returnMat;
        }
    }
}
