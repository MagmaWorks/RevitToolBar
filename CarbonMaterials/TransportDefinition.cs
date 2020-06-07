using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using Newtonsoft.Json;


namespace CarbonMaterials
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TransportDefinition
    {
        [JsonProperty]
        public string Level1 { get; private set; }
        [JsonProperty]
        public string Level2 { get; private set; }
        [JsonProperty]
        public string Level3 { get; private set; }
        [JsonProperty]
        public string Level4 { get; private set; }
        [JsonProperty]
        public double CarbonConversionFactor { get; private set; }

        static List<TransportDefinition> _definitions;
        public static List<TransportDefinition> Definitions
        {
            get
            {
                if (_definitions == null)
                {
                    _definitions = ReadTransportData();
                }
                return _definitions;
            }
        }

        public TransportDefinition()
        {

        }


        private TransportDefinition(string level1, string level2, string level3, string level4, double carbon)
        {
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            Level4 = level4;
            CarbonConversionFactor = carbon;
        }

        public static List<TransportDefinition> ReadTransportData()
        {
            var returnDefinitions = new List<TransportDefinition>();
            //string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //using (var reader = new StreamReader(assemblyFolder + @"\Resources\FreightCarbonCoefficients.csv"))
            var stream = new MemoryStream();
            var write = new StreamWriter(stream);
            write.Write(Resource1.FreightCarbonCoefficients);
            write.Flush();
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    double outValue = 0;
                    double.TryParse(values[6], out outValue);
                    TransportDefinition def = new TransportDefinition(values[0], values[1], values[2], values[3], outValue);
                    returnDefinitions.Add(def);
                }
            }
            return returnDefinitions;
        }

        public static List<string> Level1Names()
        {
            var returnList = Definitions.Select(a => a.Level1).Distinct().ToList();

            return returnList;
        }

        public static List<string> Level2Names(string level1)
        {
            var returnList = Definitions
                .Where(a => a.Level1 == level1)
                .Select(b => b.Level2)
                .Distinct()
                .ToList();

            return returnList;
        }

        public static List<string> Level3Names(string level1, string level2)
        {
            var returnList = Definitions
                .Where(a => (a.Level1 == level1) && (a.Level2 == level2))
                .Select(a => a.Level3)
                .Distinct()
                .ToList();

            return returnList;
        }

        public static List<string> Level4Names(string level1, string level2, string level3)
        {
            var returnList = Definitions
                .Where(a => (a.Level1 == level1) && (a.Level2 == level2) && (a.Level3 == level3))
                .Select(a => a.Level4)
                .Distinct()
                .ToList();

            return returnList;
        }

        public static TransportDefinition GetDefinition(string level1, string level2, string level3, string level4)
        {
            var returnValue1 = Definitions
                .Where(a => (a.Level1 == level1) && (a.Level2 == level2));

            var returnValue2 = Definitions
                .Where(a => (a.Level1 == "Freighting goods") && (a.Level2 == "HGV (all diesel)") && (a.Level3 == "Rigid (>7.5 tonnes-17 tonnes)") && (a.Level4 == "50% Laden"));

            var returnValue = Definitions
                .Where(a => (a.Level1 == level1) && (a.Level2 == level2) && (a.Level3 == level3) && (a.Level4 == level4));

            if (returnValue.Count() > 0)
            {
                var retVal = returnValue.First();
                return new TransportDefinition(retVal.Level1, retVal.Level2, retVal.Level3, retVal.Level4, retVal.CarbonConversionFactor);
            }
            else
            {
                var retVal = Definitions[0];
                return new TransportDefinition(retVal.Level1, retVal.Level2, retVal.Level3, retVal.Level4, retVal.CarbonConversionFactor);
            }

        }

        public static TransportDefinition DefaultDieselRigidHGV()
        {
            return GetDefinition("Freighting goods", "HGV (all diesel)", "Rigid (>7.5 tonnes-17 tonnes)", "50% Laden");
        }

        public static TransportDefinition DefaultDiesel33THGV()
        {
            return GetDefinition("Freighting goods", "HGV (all diesel)", "Articulated (>33t)", "50% Laden");
        }
    }
}
