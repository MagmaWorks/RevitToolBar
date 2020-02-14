using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonMaterials
{
    public class ICETimber : ICEMaterial
    {
        public override string Name
        {
            get
            {
                if (IncludeCarbonFromBiomass)
                    return _timberMaterial + " " + string.Format("{0:0.00}", TotalAtoC) + "kg CO2e/m3 including biomass";
                else
                    return _timberMaterial + " " + string.Format("{0:0.00}", TotalAtoC) + "kg CO2e/m3 exclduing biomass";
            }
        }

        double _carbonFraction = 0.5;
        public double CarbonFraction
        {
            get
            {
                return _carbonFraction;
            }
            set
            {
                _carbonFraction = value;
            }
        }

        double _moistureContent = 0.12;
        public double MoistureContent
        {
            get
            {
                return _moistureContent;
            }
            set
            {
                _moistureContent = value;
            }
        }

        public override double SequesteredCarbonDensity
        {
            get
            {
                if (IncludeSequesteredCarbon)
                    return (44d / 12d) * _carbonFraction * ((MassDensity) / (1 + _moistureContent));
                else
                    return 0;
            }
        }

        bool _includeCarbonFromBiomass;
        public bool IncludeCarbonFromBiomass
        {
            get
            {
                return _includeCarbonFromBiomass;
            }
            set
            {
                _includeCarbonFromBiomass = value;
            }
        }

        public List<string> Materials
        {
            get
            {
                return TimberMaterials;
            }
        }

        string _timberMaterial;
        public string TimberMaterial
        {
            get
            {
                return _timberMaterial;
            }
            set
            {
                _timberMaterial = value;
            }
        }

        public override GWPValue A1toA3
        {
            get
            {
                double val = 0;
                if (IncludeCarbonFromBiomass)
                    val = (TimberCarbonData["Fossil fuel portion"][_timberMaterial] + TimberCarbonData["Biomass fuel portion"][_timberMaterial]) * _massDensity;
                else
                    val = TimberCarbonData["Fossil fuel portion"][_timberMaterial] * _massDensity;

                if (IncludeSequesteredCarbon)
                    val -= SequesteredCarbonDensity;

                return new GWPValue(val);
            }
        }

        public override GWPValue C3
        {
            get
            {
                if (IncludeSequesteredCarbon)
                    return new GWPValue(RecyclingReuseFactor + SequesteredCarbonDensity);
                else
                    return new GWPValue(RecyclingReuseFactor);
            }
        }

        public ICETimber(string material, bool includeBiomass)
        {
            _timberMaterial = material;
            _includeCarbonFromBiomass = includeBiomass;
            Category = ICECategory.Timber;
            _massDensity = DefaultMassDensity[material];
        }

        public ICETimber()
        {
            _timberMaterial = "General";
            _includeCarbonFromBiomass = true;
            Category = ICECategory.Timber;
            _massDensity = DefaultMassDensity["General"];
        }

        public static Dictionary<string, Dictionary<string, double>> TimberCarbonData = new Dictionary<string, Dictionary<string, double>>
        {
            {"Fossil fuel portion", new Dictionary<string, double>
            { {"General",0.31},{"Glue Laminated timber ",0.42},{"Hardboard (High Density Fibreboard)",0.58},{"Laminated Veneer Lumber",0.33},{"Medium Density Fibreboard (MDF)",0.39},{"Oriented Strand Board (OSB) ",0.45},{"Particle Board",0.54},{"Plywood",0.45},{"Sawn Hardwood",0.24},{"Sawn Softwood",0.2}}
            },
            {"Biomass fuel portion", new Dictionary<string, double>
            {{"General",0.41},{"Glue Laminated timber ",0.45},{"Hardboard (High Density Fibreboard)",0.51},{"Laminated Veneer Lumber",0.32},{"Medium Density Fibreboard (MDF)",0.35},{"Oriented Strand Board (OSB) ",0.54},{"Particle Board",0.32},{"Plywood",0.65},{"Sawn Hardwood",0.63},{"Sawn Softwood",0.39}}
            }
        };

        public static Dictionary<string, double> DefaultMassDensity = new Dictionary<string, double>
        { {"General",600},{"Glue Laminated timber ",600},{"Hardboard (High Density Fibreboard)",600},{"Laminated Veneer Lumber",600},{"Medium Density Fibreboard (MDF)",600},{"Oriented Strand Board (OSB) ",600},{"Particle Board",600},{"Plywood",600},{"Sawn Hardwood",600},{"Sawn Softwood",600} };

        public static List<string> TimberMaterials = new List<string> {"General","Glue Laminated timber ","Hardboard (High Density Fibreboard)","Laminated Veneer Lumber","Medium Density Fibreboard (MDF)","Oriented Strand Board (OSB) ","Particle Board","Plywood","Sawn Hardwood","Sawn Softwood",
        };

        public override GWPMaterial getCopy()
        {
            var returnMaterial = new ICETimber(this.TimberMaterial, this.IncludeCarbonFromBiomass);
            returnMaterial._carbonFraction = this.CarbonFraction;
            returnMaterial._moistureContent = this._moistureContent;
            returnMaterial._timberMaterial = this._timberMaterial;
            returnMaterial.ConstructionFactor = this.ConstructionFactor;
            returnMaterial.DeConstructionFactor = this.DeConstructionFactor;
            returnMaterial.InUseFactor = this.InUseFactor;
            returnMaterial._massDensity = this.MassDensity;
            returnMaterial.IncludeSequesteredCarbon = this.IncludeSequesteredCarbon;

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

        public override void copyMaterialFrom(GWPMaterial material)
        {
            if (material is ICETimber)
            {
                var m = material as ICETimber;
                this.TimberMaterial = m.TimberMaterial;
                this.IncludeCarbonFromBiomass = m.IncludeCarbonFromBiomass;
                this._carbonFraction = m.CarbonFraction;
                this._moistureContent = m.MoistureContent;
                this.ConstructionFactor = m.ConstructionFactor;
                this.DeConstructionFactor = m.DeConstructionFactor;
                this.InUseFactor = m.InUseFactor;
                this._massDensity = m.MassDensity;
                this.IncludeSequesteredCarbon = m.IncludeSequesteredCarbon;
                this.TransportsToSite.Clear();
                this.TransportsToDispoal.Clear();

                foreach (var item in m.TransportsToSite)
                {
                    this.TransportsToSite.Add(item);
                }
                foreach (var item in m.TransportsToDispoal)
                {
                    this.TransportsToDispoal.Add(item);
                }
            }
        }
    }
}
