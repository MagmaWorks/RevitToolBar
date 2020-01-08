using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    public class ICEMaterialTimber : ICEMaterial
    {
        public override string Name
        {
            get
            {
                if (IncludeCarbonFromBiomass)
                    return _timberMaterial + " " + CarbonDensity + "kg/m3 including biomass";
                else
                    return _timberMaterial + " " + CarbonDensity + "kg/m3 exclduing biomass";
            }
        }

        public override double CarbonDensity
        {
            get
            {
                if (IncludeCarbonFromBiomass)
                    return (TimberCarbonData["Fossil fuel portion"][_timberMaterial] + TimberCarbonData["Biomass fuel portion"][_timberMaterial]) * _massDensity;
                else
                    return TimberCarbonData["Fossil fuel portion"][_timberMaterial] * _massDensity;
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
                RaisePropertyChanged(nameof(CarbonFraction));
                RaisePropertyChanged(nameof(CarbonDensity));
                RaisePropertyChanged(nameof(Name));
                Parent.ICEMaterialUpdated();
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
                RaisePropertyChanged(nameof(MoistureContent));
                RaisePropertyChanged(nameof(CarbonDensity));
                RaisePropertyChanged(nameof(Name));
                Parent.ICEMaterialUpdated();
            }
        }

        public override double SequesteredCarbonDensity
        {
            get
            {
                return (44d / 12d) * _carbonFraction * ((_massDensity) / (1 + _moistureContent));
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
                RaisePropertyChanged(nameof(IncludeCarbonFromBiomass));
                RaisePropertyChanged(nameof(CarbonDensity));
                RaisePropertyChanged(nameof(Name));
                Parent.ICEMaterialUpdated();
            }
        }

        double _massDensity;
        public double MassDensity
        {
            get
            {
                return _massDensity;
            }
            set
            {
                _massDensity = value;
                RaisePropertyChanged(nameof(MassDensity));
                RaisePropertyChanged(nameof(CarbonDensity));
                RaisePropertyChanged(nameof(Name));
                Parent.ICEMaterialUpdated();

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
                RaisePropertyChanged(nameof(TimberMaterial));
                RaisePropertyChanged(nameof(CarbonDensity));
                Parent.ICEMaterialUpdated();
            }
        }

        public ICEMaterialTimber(string material, bool includeBiomass, ElementCarbonVM parent)
        {
            _timberMaterial = material;
            _includeCarbonFromBiomass = includeBiomass;
            Category = ICECategory.Timber;
            _massDensity = DefaultMassDensity[material];
            Parent = parent;
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
    }
}
