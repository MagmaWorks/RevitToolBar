using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GWPValue
    {
        [JsonProperty]
        public double Value { get; }
        [JsonProperty]
        public bool Assigned { get; }
        public GWPValue(double value)
        {
            Value = value;
            Assigned = true;
        }

        public GWPValue(double value, bool isAssigned)
        {
            Value = value;
            Assigned = isAssigned;
        }

        public GWPValue()
        {
            Value = 0;
            Assigned = false;
        }
    }
}
