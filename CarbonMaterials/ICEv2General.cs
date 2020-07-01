using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ICEv2General : ICEMaterial
    {
        public override string Name => _materialFamily + ": " + _material + " " + A1toA3 + "kg/m3";

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

        //public override double MassDensity { get => _massDensity; protected set => _massDensity = value; }

        public override GWPValue A1toA3
        {
            get
            {
                return new GWPValue(_embodiedCarbon * MassDensity);
            }
        }

        public void UpdateValues(string material, string submaterial, double embodiedCarbon, string guid)
        {
            _materialFamily = material;
            _material = submaterial;
            _embodiedCarbon = embodiedCarbon;
            _reference = guid;
        }

        [JsonProperty(PropertyName = "Reference")]
        string _reference;
        public string Reference { get => _reference; }

        public override double SequesteredCarbonDensity => 0;

        [JsonProperty(PropertyName = "MaterialFamily")]
        string _materialFamily;
        [JsonProperty(PropertyName = "Material")]
        string _material;

        public ICEv2General(string material, string submaterial, double embodiedCarbon, string guid)
        {
            _materialFamily = material;
            _material = submaterial;
            _embodiedCarbon = embodiedCarbon;
            _massDensity = 0;
            _reference = guid;
            Category = ICECategory.GeneralV2;
        }

        public ICEv2General()
        {
            _materialFamily = "Plaster";
            _material = "Plasterboard";
            _embodiedCarbon = 0;
            _massDensity = 0;
            _reference = "Plaster Plasterboard";
            Category = ICECategory.GeneralV2;
        }

        public static List<ICEv2DBMaterial> ReadICEv2Materials()
        {
            byte[] jsonToRead = Resource1.ICEv2;
            StreamReader reader = new StreamReader(new MemoryStream(jsonToRead));
            JsonSerializer serialiser = new JsonSerializer();
            var test = ((List<ICEv2DBMaterial>)serialiser.Deserialize(reader, typeof(List<ICEv2DBMaterial>)));
            return test;
        }

        public class ICEv2DBMaterial
        {
            public string Reference { get; set; }
            public string MaterialFamily { get; set; }
            public string Material { get; set; }
            public string Notes { get; set; }
            public double EmbodiedCarbon { get; set; }
        }

        public static ICEv2General GetInsulation()
        {
            var dBase = ReadICEv2Materials().Where(a => a.Reference == "Insulation Rockwool").First();
            var returnMat = new ICEv2General(dBase.MaterialFamily, dBase.Material, dBase.EmbodiedCarbon, dBase.Reference);
            returnMat.MassDensity = 2.2;
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
