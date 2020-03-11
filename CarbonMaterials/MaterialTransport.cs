using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MaterialTransport
    {
        [JsonProperty]
        public TransportDefinition Definition { get; set; }

        [JsonProperty]
        public double Distance { get; set; }

        public double CarbonPerKG
        {
            get
            {
                return Definition.CarbonConversionFactor * Distance / 1000; // convert from tonnes to kg
            }
        }

        public MaterialTransport()
        {
            
        }

        public static MaterialTransport DefaultDieselRigidHGV()
        {
            return new MaterialTransport() { Distance = 15, Definition = TransportDefinition.DefaultDieselRigidHGV() };
        }
    }
}
