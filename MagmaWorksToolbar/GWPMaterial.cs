using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    public abstract class GWPMaterial : ViewModelBase
    {
        public virtual GWPValue A1 { get; } = new GWPValue();
        public virtual GWPValue A2 { get; } = new GWPValue();
        public virtual GWPValue A3 { get; } = new GWPValue();
        public virtual GWPValue A4 { get; } = new GWPValue();
        public virtual GWPValue A5 { get; } = new GWPValue();
        public virtual GWPValue A1toA3 { get; } = new GWPValue();
        public virtual GWPValue B1 { get; } = new GWPValue();
        public virtual GWPValue B2 { get; } = new GWPValue();
        public virtual GWPValue B3 { get; } = new GWPValue();
        public virtual GWPValue B4 { get; } = new GWPValue();
        public virtual GWPValue B5 { get; } = new GWPValue();
        public virtual GWPValue B6 { get; } = new GWPValue();
        public virtual GWPValue B7 { get; } = new GWPValue();
        public virtual GWPValue C1 { get; } = new GWPValue();
        public virtual GWPValue C2 { get; } = new GWPValue();
        public virtual GWPValue C3 { get; } = new GWPValue();
        public virtual GWPValue C4 { get; } = new GWPValue();

        public double TotalA => A1toA3.Value + A2.Value + A3.Value;
        public double TotalB => B1.Value + B2.Value + B3.Value + B4.Value + B5.Value + B6.Value + B7.Value;
        public double TotalC => C1.Value + C2.Value + C3.Value + C4.Value;
        public double TotalAtoC => TotalA + TotalB + TotalC;

        public abstract string Name { get; }
        public abstract double CarbonDensity { get; }
        public abstract double SequesteredCarbonDensity { get; }
        public abstract ElementCarbonVM Parent { get; }
        public abstract GWPMaterialType GWPMaterialType { get; }
        public abstract GWPMaterial copyMaterial();
    }

    public class TransportDefinition
    {
        public string Level1 { get; }
        public string Level2 { get; }
        public string Level3 { get; }
        public string Level4 { get; }
        public double CarbonConversionFactor { get; }
        public static List<TransportDefinition> Definitions;

        private TransportDefinition(string level1, string level2, string level3, string level4,double carbon)
        {
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            Level4 = level4;
            CarbonConversionFactor = carbon;
        }

        public static void ReadTransportData()
        {
            Definitions = new List<TransportDefinition>();
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (var reader = new StreamReader(assemblyFolder + @"\FreightCarbonCoefficients.csv"))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    double outValue = 0;
                    double.TryParse(values[6], out outValue);
                    TransportDefinition def = new TransportDefinition(values[0], values[1], values[2], values[3], outValue);
                    Definitions.Add(def);
                }
            }
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
                .ToList();

            return returnList;
        }

        public static List<string> Level3Names(string level1, string level2)
        {
            var returnList = Definitions
                .Where(a => (a.Level1 == level1) && (a.Level2 == level2))
                .Select(a => a.Level3)
                .ToList();

            return returnList;
        }

        public static List<string> Level4Names(string level1, string level2, string level3)
        {
            var returnList = Definitions
                .Where(a => (a.Level1 == level1) && (a.Level2 == level2) && (a.Level3 == level3))
                .Select(a => a.Level3)
                .ToList();

            return returnList;
        }
    }

    public class GWPValue
    {
        public double Value { get; }
        public bool Assigned { get; }
        public GWPValue(double value)
        {
            Value = value;
            Assigned = true;
        }

        public GWPValue()
        {
            Value = 0;
            Assigned = false;
        }
    }

    public class GWPMaterialNone : GWPMaterial
    {
        public override string Name => "None";

        public override double CarbonDensity => 0;

        public override double SequesteredCarbonDensity => 0;

        public override GWPMaterialType GWPMaterialType { get; }

        public override ElementCarbonVM Parent { get; }

        public GWPMaterialNone(ElementCarbonVM parent)
        {
            GWPMaterialType = GWPMaterialType.None;
            Parent = parent;
        }

        public override GWPMaterial copyMaterial()
        {
            return new GWPMaterialNone(this.Parent);
        }
    }

    public enum GWPMaterialType
    {
        None,
        General,
        ICE
    }

    public abstract class ICEMaterial2 : GWPMaterial
    {
        public ICECategory Category { get; protected set; }

        public static List<string> MaterialTypes = new List<string> { "None", "Concrete", "Steel", "Timber" };
    }

    public class ICEConcrete : ICEMaterial2
    {
        string _grade;
        public string Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                _grade = value;
                RaisePropertyChanged(nameof(Grade));
                RaisePropertyChanged(nameof(CarbonDensity));
                RaisePropertyChanged(nameof(Name));
                Parent.ICEMaterialUpdated();
            }
        }

        public override double SequesteredCarbonDensity => 0;

        public override string Name
        {
            get
            {
                return _grade + " " + _replacement + " " + _rebar + "kg/m3";
            }
        }

        string _replacement;
        public string Replacement
        {
            get
            {
                return _replacement;
            }
            set
            {
                _replacement = value;
                RaisePropertyChanged(nameof(Replacement));
                RaisePropertyChanged(nameof(CarbonDensity));
                RaisePropertyChanged(nameof(Name));
                Parent.ICEMaterialUpdated();
            }
        }

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
                RaisePropertyChanged(nameof(ReinforcementDensity));
                RaisePropertyChanged(nameof(CarbonDensity));
                RaisePropertyChanged(nameof(Name));
                Parent.ICEMaterialUpdated();
            }
        }

        public List<string> Replacements
        {
            get
            {
                return ConcreteReplacements;
            }
        }

        public List<string> Grades
        {
            get
            {
                return ConcreteGrades;
            }
        }

        public override double CarbonDensity
        {
            get
            {
                double carbonDensity = CarbonData[_replacement][_grade];
                double carbonDensityWithRebar = carbonDensity + 0.077 * (_rebar / 100d);
                double rcMass = 2500;
                return rcMass * carbonDensityWithRebar;
            }
        }

        public override GWPValue A1toA3
        {
            get
            {
                return new GWPValue(CarbonData[_replacement][_grade]);
            }
        }

        public override GWPValue A4
        {
            get
            {
                return new GWPValue(0);
            }
        }

        double _transportDistanceToSite = 0;
        public double TransportDistanceToSite
        {
            get
            {
                return _transportDistanceToSite;
            }
        }

        TransportDefinition _transportToSite;
        public TransportDefinition TransportToSite
        {
            get
            {
                return _transportToSite;
            }
        }

        TransportDefinition _transportToDisposal;
        public TransportDefinition TransportToDisposal
        {
            get
            {
                return _transportToDisposal;
            }
        }

        double _transportDistanceToDisposal = 0;
        public double TransportDistanceToDisposal
        {
            get
            {
                return _transportDistanceToDisposal;
            }
        }

        public override ElementCarbonVM Parent { get; }

        public override GWPMaterialType GWPMaterialType { get; }

        public ICEConcrete(string grade, string replacement, double rebarDensity, ElementCarbonVM parent)
        {
            _grade = grade;
            _replacement = replacement;
            _rebar = rebarDensity;
            Category = ICECategory.Concrete;
            Parent = parent;
        }

        public static Dictionary<string, Dictionary<string, double>> CarbonData = new Dictionary<string, Dictionary<string, double>>
            {
                {"zero", new Dictionary<string, double>
                    {{"GEN0",0.076},{"GEN1",0.104},{"GEN2",0.114},{"GEN3",0.123},{"RC20/25",0.132},{"RC25/30",0.14},{"RC28/35",0.148},{"RC32/40",0.163},{"RC40/50",0.188},{"PAV1",0.148},{"PAV2",0.163}}
                },
                {"15%PFA", new Dictionary<string, double>
                    {{"GEN0",0.069},{"GEN1",0.094},{"GEN2",0.105},{"GEN3",0.112},{"RC20/25",0.122},{"RC25/30",0.13},{"RC28/35",0.138},{"RC32/40",0.152},{"RC40/50",0.174},{"PAV1",0.138},{"PAV2",0.152} }
                },
                {"30%PFA", new Dictionary<string, double>
                {{"GEN0",0.061},{"GEN1",0.082},{"GEN2",0.093},{"GEN3",0.1},{"RC20/25",0.108},{"RC25/30",0.115},{"RC28/35",0.124},{"RC32/40",0.136},{"RC40/50",0.155},{"PAV1",0.123},{"PAV2",0.137} }
                },
                {"25%GGBS", new Dictionary<string, double>
                {{"GEN0",0.06},{"GEN1",0.08},{"GEN2",0.088},{"GEN3",0.096},{"RC20/25",0.104},{"RC25/30",0.111},{"RC28/35",0.119},{"RC32/40",0.133},{"RC40/50",0.153},{"PAV1",0.118},{"PAV2",0.133} }
                },
                {"50%GGBS", new Dictionary<string, double>
                {{"GEN0",0.045},{"GEN1",0.058},{"GEN2",0.065},{"GEN3",0.07},{"RC20/25",0.077},{"RC25/30",0.081},{"RC28/35",0.088},{"RC32/40",0.1},{"RC40/50",0.115},{"PAV1",0.088},{"PAV2",0.1} }
                }
            };

        public static List<string> ConcreteGrades = new List<string> { "GEN0", "GEN1", "GEN2", "GEN3", "RC20/25", "RC25/30", "RC28/35", "RC32/40", "RC40/50", "PAV1", "PAV2" };
        public static List<string> ConcreteReplacements = new List<string> { "zero", "15%PFA", "30%PFA", "25%GGBS", "50%GGBS" };

        public override GWPMaterial copyMaterial()
        {
            return new ICEConcrete(this.Grade, this.Replacement, this.ReinforcementDensity, this.Parent);
        }
    }
}
