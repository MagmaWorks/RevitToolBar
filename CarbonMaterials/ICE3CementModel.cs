using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ICE3ConcreteModel : ICEMaterial
    {
        [JsonProperty(PropertyName = "Constituents")]
        public List<CementAndConcreteConstituent> Constituents;
        public Dictionary<string, CementAndConcreteMaterial> AllConstituents;

        [JsonProperty(PropertyName = "TransportsToProcessing")]
        List<MaterialTransport> _transportsToProcessing = new List<MaterialTransport>();
        public List<MaterialTransport> TransportsToProcessing
        {
            get
            {
                return _transportsToProcessing;
            }
        }

        [JsonProperty(PropertyName = "ReinforcementDensity")]
        double _rebar;
        public double ReinforcementDensity
        {
            get
            {
                return _rebar;
            }
            set
            {
                _rebar = value;
            }
        }

        public override double SequesteredCarbonDensity => 0;

        public override void copyMaterialFrom(GWPMaterial material)
        {
            if (material is ICE3ConcreteModel)
            {
                var m = material as ICE3ConcreteModel;
                this.ReinforcementDensity = m.ReinforcementDensity;
                this.ConstructionFactor = m.ConstructionFactor;
                this.DeConstructionFactor = m.DeConstructionFactor;
                this.InUseFactor = m.InUseFactor;
                this._massDensity = m.MassDensity;
                this.IncludeSequesteredCarbon = m.IncludeSequesteredCarbon;
                this.TransportsToSite.Clear();
                this.TransportsToDispoal.Clear();
                this.Constituents.Clear();

                foreach (var item in m.TransportsToSite)
                {
                    this.TransportsToSite.Add(item);
                }
                foreach (var item in m.TransportsToDispoal)
                {
                    this.TransportsToDispoal.Add(item);
                }
                foreach (var item in m.Constituents)
                {
                    this.Constituents.Add(item);
                }
            }
        }

        public override GWPMaterial getCopy()
        {
            var returnMaterial = new ICE3ConcreteModel();

            // TO BE COMPLETED

            returnMaterial.ConstructionFactor = this.ConstructionFactor;
            returnMaterial.DeConstructionFactor = this.DeConstructionFactor;
            returnMaterial.InUseFactor = this.InUseFactor;
            returnMaterial.IncludeSequesteredCarbon = this.IncludeSequesteredCarbon;
            returnMaterial._massDensity = this.MassDensity;
            foreach (var item in this.TransportsToSite)
            {
                returnMaterial.TransportsToSite.Add(item);
            }
            foreach (var item in this.TransportsToDispoal)
            {
                returnMaterial.TransportsToDispoal.Add(item);
            }
            return returnMaterial;
        }

        [JsonProperty(PropertyName = "ProcessingFactor")]
        double _processing = 0.00072;
        public double Processing
        {
            get
            {
                return _processing;
            }
            set
            {
                _processing = value;
            }
        }

        [JsonProperty(PropertyName = "Wastage")]
        double _materialWastage = .01;
        public double Wastage
        {
            get
            {
                return _materialWastage;
            }
            set
            {
                _materialWastage = value;
            }
        }

        public override GWPValue A1toA3
        {
            get
            {
                double carbon = Constituents.Sum(a => a.Proportion * a.Material.EmbodiedCarbon);
                double transport = _transportsToProcessing.Sum(a => a.CarbonPerKG);
                double steel = 0.73 * _rebar / 2500;
                return new GWPValue((carbon * (1 + _materialWastage) + _processing + steel + transport) * _massDensity);
            }
        }

        public override string Name
        {
            get
            {
                return "ICEv3 Concrete" + string.Format("{0:0.00}", TotalAtoC) + "kg CO2e/m3"; ;
            }
        }

        public ICE3ConcreteModel()
        {
            AllConstituents = ICE3CementModel.getCementConstituents();
            _massDensity = 2350;
            Category = ICECategory.ConcreteV3;
            TransportsToProcessing.Add(MaterialTransport.Default33THGV());
            Constituents = new List<CementAndConcreteConstituent>
            {
                //new CementAndConcreteConstituent{Material = new ICE3CementModel(), Proportion = 0.15},
                //new CementAndConcreteConstituent{Material = AllConstituents["Aggregates"], Proportion = 0.8685},
                //new CementAndConcreteConstituent{Material = AllConstituents["Admixture"], Proportion = 0.0015},
                //new CementAndConcreteConstituent{Material = AllConstituents["Water"], Proportion = 0.03}
            };
        }

        public static ICE3ConcreteModel DefaultRC4050Concrete()
        {
            var allConstituents = ICE3CementModel.getCementConstituents(); 

            return new ICE3ConcreteModel
            {
                Constituents = new List<CementAndConcreteConstituent>
                {
                    new CementAndConcreteConstituent{Material = ICE3CementModel.getOPC(), Proportion=0.179},
                    new CementAndConcreteConstituent{Material = allConstituents["Aggregates"], Proportion = 0.767},
                    new CementAndConcreteConstituent{Material = allConstituents["Admixture"], Proportion = 0},
                    new CementAndConcreteConstituent{Material = allConstituents["Water"], Proportion = 0.054}
                }
            };
        }

        public static ICE3ConcreteModel DefaultRC4050ConcreteGGBS20()
        {
            var allConstituents = ICE3CementModel.getCementConstituents();

            return new ICE3ConcreteModel
            {
                Constituents = new List<CementAndConcreteConstituent>
                {
                    new CementAndConcreteConstituent{Material = ICE3CementModel.getOPC(), Proportion=0.143},
                    new CementAndConcreteConstituent{Material = allConstituents["GGBS"], Proportion=0.036},
                    new CementAndConcreteConstituent{Material = allConstituents["Aggregates"], Proportion = 0.767},
                    new CementAndConcreteConstituent{Material = allConstituents["Admixture"], Proportion = 0},
                    new CementAndConcreteConstituent{Material = allConstituents["Water"], Proportion = 0.054}
                }
            };
        }

        public static ICE3ConcreteModel DefaultRC3240Concrete()
        {
            var allConstituents = ICE3CementModel.getCementConstituents();

            return new ICE3ConcreteModel
            {
                Constituents = new List<CementAndConcreteConstituent>
                {
                    new CementAndConcreteConstituent{Material = ICE3CementModel.getOPC(), Proportion=0.153},
                    new CementAndConcreteConstituent{Material = allConstituents["Aggregates"], Proportion = 0.801},
                    new CementAndConcreteConstituent{Material = allConstituents["Admixture"], Proportion = 0},
                    new CementAndConcreteConstituent{Material = allConstituents["Water"], Proportion = 0.046}
                }
            };
        }

        public static ICE3ConcreteModel DefaultRC3240ConcreteGGBS20()
        {
            var allConstituents = ICE3CementModel.getCementConstituents();

            return new ICE3ConcreteModel
            {
                Constituents = new List<CementAndConcreteConstituent>
                {
                    new CementAndConcreteConstituent{Material = ICE3CementModel.getOPC(), Proportion=0.122},
                    new CementAndConcreteConstituent{Material = allConstituents["GGBS"], Proportion=0.031},
                    new CementAndConcreteConstituent{Material = allConstituents["Aggregates"], Proportion = 0.801},
                    new CementAndConcreteConstituent{Material = allConstituents["Admixture"], Proportion = 0},
                    new CementAndConcreteConstituent{Material = allConstituents["Water"], Proportion = 0.046}
                }
            };
        }

        public static ICE3ConcreteModel DefaultRC2025oncrete()
        {
            var allConstituents = ICE3CementModel.getCementConstituents();

            return new ICE3ConcreteModel
            {
                Constituents = new List<CementAndConcreteConstituent>
                {
                    new CementAndConcreteConstituent{Material = ICE3CementModel.getOPC(), Proportion=0.121},
                    new CementAndConcreteConstituent{Material = allConstituents["Aggregates"], Proportion = 0.843},
                    new CementAndConcreteConstituent{Material = allConstituents["Admixture"], Proportion = 0},
                    new CementAndConcreteConstituent{Material = allConstituents["Water"], Proportion = 0.036}
                }
            };
        }
    }

    public interface IConstituentMaterial
    {
        string Name { get; }
        double EmbodiedCarbon { get; }
        string Notes { get; }
        Dictionary<string, CementAndConcreteMaterial> AllConstituents { get; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ICE3CementModel : IConstituentMaterial
    {
        [JsonProperty(PropertyName = "Constituents")]
        public List<CementAndConcreteConstituent> Constituents;
        public Dictionary<string, CementAndConcreteMaterial> AllConstituents { get; private set; }

        string _name = "Cement";
        public string Name => _name;

        public string Notes
        {
            get
            {
                string returnString = "";
                foreach (var item in Constituents)
                {
                    returnString += item.Material.Notes + " "; 
                }
                return returnString;
            }
        }

        public ICE3CementModel()
        {
            AllConstituents = getCementConstituents();

            Constituents = new List<CementAndConcreteConstituent>
            {
                //new CementAndConcreteConstituent{Material = AllConstituents["Clinker"], Proportion=0.94},
                //new CementAndConcreteConstituent{Material = AllConstituents["Gypsum"], Proportion=0.05},
                //new CementAndConcreteConstituent{Material = AllConstituents["MAC"], Proportion=0.01 }
            };

            _name = "OPC";
        }

        public static ICE3CementModel getOPC()
        {
            var allConstituents = getCementConstituents();

            return new ICE3CementModel
            {
                Constituents = new List<CementAndConcreteConstituent>
                {
                    new CementAndConcreteConstituent{Material = allConstituents["Clinker"], Proportion=0.94},
                    new CementAndConcreteConstituent{Material = allConstituents["Gypsum"], Proportion=0.05},
                    new CementAndConcreteConstituent{Material = allConstituents["MAC"], Proportion=0.01 }
                }
            };
        }

        /// <summary>
        /// Returns A1 carbon kg CO2e per kg
        /// </summary>
        public double EmbodiedCarbon => Constituents.Sum(a => a.Proportion * a.Material.EmbodiedCarbon);

        public static Dictionary<string, CementAndConcreteMaterial> getCementConstituents()
        {
            byte[] jsonToRead = Resource1.CementConstituents;
            StreamReader reader = new StreamReader(new MemoryStream(jsonToRead));
            JsonSerializer serialiser = new JsonSerializer();
            var test = ((List<CementAndConcreteMaterial>)serialiser.Deserialize(reader, typeof(List<CementAndConcreteMaterial>)));
            return test.ToDictionary(a => a.Name);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CementAndConcreteMaterial : IConstituentMaterial
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get;  set; }
        [JsonProperty(PropertyName = "EmbodiedCarbon")]
        public double EmbodiedCarbon { get;  set; }
        [JsonProperty(PropertyName = "Notes")]
        public string Notes { get;  set; }
        Dictionary<string, CementAndConcreteMaterial> _allConstituents;
        public Dictionary<string, CementAndConcreteMaterial> AllConstituents
        {
            get
            {
                if (_allConstituents == null)
                {
                    _allConstituents = ICE3CementModel.getCementConstituents();
                }
                return _allConstituents;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CementAndConcreteConstituent
    {
        [JsonProperty(PropertyName = "Material")]
        public IConstituentMaterial Material { get; set; }

        [JsonProperty(PropertyName = "Proportion")]
        public double Proportion { get; set; }
    }
}
