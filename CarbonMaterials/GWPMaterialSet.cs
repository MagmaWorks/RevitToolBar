using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace CarbonMaterials
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public class GWPMaterialSet
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public List<GWPMaterial> Materials { get; private set; }

        private GWPMaterialSet()
        {

        }

        public GWPMaterialSet(string name)
        {
            Name = name;
            Materials = new List<GWPMaterial>
            {
                new ICENone()
            };
        }

        public GWPMaterialSet(string name, List<GWPMaterial> materials)
        {
            Name = name;
            Materials = materials;
        }

        public double A1 => Materials.Sum(a => a.A1.Value);
        public double A2 => Materials.Sum(a => a.A2.Value);
        public double A3 => Materials.Sum(a => a.A3.Value);
        public double A1toA3 => Materials.Sum(a => a.A1toA3.Value);
        public double A4 => Materials.Sum(a => a.A4.Value);
        public double A5 => Materials.Sum(a => a.A5.Value);
        public double B1 => Materials.Sum(a => a.B1.Value);
        public double B2 => Materials.Sum(a => a.B2.Value);
        public double B3 => Materials.Sum(a => a.B3.Value);
        public double B4 => Materials.Sum(a => a.B4.Value);
        public double B5 => Materials.Sum(a => a.B5.Value);
        public double B6 => Materials.Sum(a => a.B6.Value);
        public double B7 => Materials.Sum(a => a.B7.Value);
        public double C1 => Materials.Sum(a => a.C1.Value);
        public double C2 => Materials.Sum(a => a.C2.Value);
        public double C3 => Materials.Sum(a => a.C3.Value);
        public double C4 => Materials.Sum(a => a.C4.Value);
        public double TotalAtoC => Materials.Sum(a => a.TotalAtoC);
        public double TotalA => Materials.Sum(a => a.TotalA);
        public double TotalB=> Materials.Sum(a => a.TotalB);
        public double TotalC => Materials.Sum(a => a.TotalC);

        public static GWPMaterialSet GetDefaultGWPConcreteSet()
        {
            var returnSet = new GWPMaterialSet("Mass concrete", 
                new List<GWPMaterial>
                {
                    new ICEConcrete("RC32/40", "zero", 0)
                });
            return returnSet;
        }

        public Measurement SpatialDimensions { get; set; } = Measurement.Volume;

        public static GWPMaterialSet GetDefaultGWPConcreteSetSeparateSteel()
        {
            var conc = new ICEConcrete("RC32/40", "zero", 0);
            conc.TransportsToSite.Clear();
            conc.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.TransportsToDispoal.Clear();
            conc.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.ConstructionFactor = 2;
            conc.DeConstructionFactor = 1;

            var steel = new ICESteel("Bar & rod");
            steel.ChangeMassDensity(150);

            var returnSet = new GWPMaterialSet("Reinforced concrete",
                new List<GWPMaterial>
                {
                    conc,
                    steel
                });
            return returnSet;
        }

        public static GWPMaterialSet GetDefaultStructuralSteel()
        {
            var steel = new ICESteel("Section");
            steel.TransportsToSite.Clear();
            steel.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            steel.TransportsToDispoal.Clear();
            steel.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            steel.ConstructionFactor = 2;
            steel.DeConstructionFactor = 1;
            return new GWPMaterialSet("Default structural steel",
                new List<GWPMaterial>
                {
                    steel
                });
        }

        public static GWPMaterialSet GetDefaultGlulam()
        {
            var timber = new ICETimber("Glue Laminated timber ", false);
            timber.TransportsToSite.Clear();
            timber.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            timber.TransportsToDispoal.Clear();
            timber.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            timber.ConstructionFactor = 2;
            timber.DeConstructionFactor = 1;
            return new GWPMaterialSet("Default glulam",
                new List<GWPMaterial>
                {
                    timber
                });
        }

        public static GWPMaterialSet GetDefaultFoundationRC()
        {
            var conc = new ICEConcrete("RC25/30", "25%GGBS", 120);
            conc.TransportsToSite.Clear();
            conc.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.TransportsToDispoal.Clear();
            conc.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.ConstructionFactor = 2;
            conc.DeConstructionFactor = 1;
            return new GWPMaterialSet("Default foundation RC",
                new List<GWPMaterial>
                {
                    conc
                });
        }

        public static GWPMaterialSet GetDefaultRC4050()
        {
            var conc = ICE3ConcreteModel.DefaultRC4050Concrete();
            conc.ReinforcementDensity = 150;
            conc.TransportsToSite.Clear();
            conc.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.TransportsToDispoal.Clear();
            conc.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.ConstructionFactor = 2;
            conc.DeConstructionFactor = 1;
            return new GWPMaterialSet("RC 40/50",
                new List<GWPMaterial>
                {
                    conc
                });
        }

        public static GWPMaterialSet GetDefaultRC4050GGBS20()
        {
            var conc = ICE3ConcreteModel.DefaultRC4050ConcreteGGBS20();
            conc.ReinforcementDensity = 150;
            conc.TransportsToSite.Clear();
            conc.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.TransportsToDispoal.Clear();
            conc.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.ConstructionFactor = 2;
            conc.DeConstructionFactor = 1;
            return new GWPMaterialSet(@"RC 40/50, 20% GGBS",
                new List<GWPMaterial>
                {
                    conc
                });
        }

        public static GWPMaterialSet GetDefaultRC3240()
        {
            var conc = ICE3ConcreteModel.DefaultRC3240Concrete();
            conc.ReinforcementDensity = 150;
            conc.TransportsToSite.Clear();
            conc.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.TransportsToDispoal.Clear();
            conc.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.ConstructionFactor = 2;
            conc.DeConstructionFactor = 1;
            return new GWPMaterialSet("RC 32/40",
                new List<GWPMaterial>
                {
                    conc
                });
        }

        public static GWPMaterialSet GetDefaultRC3240GGBS20()
        {
            var conc = ICE3ConcreteModel.DefaultRC3240ConcreteGGBS20();
            conc.ReinforcementDensity = 150;
            conc.TransportsToSite.Clear();
            conc.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.TransportsToDispoal.Clear();
            conc.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.ConstructionFactor = 2;
            conc.DeConstructionFactor = 1;
            return new GWPMaterialSet(@"RC 32/40, 20% GGBS",
                new List<GWPMaterial>
                {
                    conc
                });
        }

        public static GWPMaterialSet GetDefaultRC2025()
        {
            var conc = ICE3ConcreteModel.DefaultRC2025oncrete();
            conc.ReinforcementDensity = 150;
            conc.TransportsToSite.Clear();
            conc.TransportsToSite.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.TransportsToDispoal.Clear();
            conc.TransportsToDispoal.Add(MaterialTransport.DefaultDieselRigidHGV());
            conc.ConstructionFactor = 2;
            conc.DeConstructionFactor = 1;
            return new GWPMaterialSet("RC 20/25",
                new List<GWPMaterial>
                {
                    conc
                });
        }

        public static GWPMaterialSet GetDefaultBrickAndSteelStudWall()
        {
            var block = ICEv3General.GetBlockwork100mm();
            var studs = ICEv3General.GetSteelStud20kg();
            var insulation = ICEv2General.GetInsulation();
            block.ConstructionFactor = 0.2;
            block.DeConstructionFactor = 0.1;
            studs.ConstructionFactor = 0.1;
            studs.DeConstructionFactor = 0.05;
            return new GWPMaterialSet("Brick and stud cavity wall",
            new List<GWPMaterial>
            {
                block,
                insulation,
                studs
            })
            { SpatialDimensions = Measurement.Area };
        }
    }
}
