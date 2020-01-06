using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    public class ICEMaterialConcrete : ICEMaterial
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

        public ICEMaterialConcrete(string grade, string replacement, double rebarDensity, ElementCarbonVM parent)
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
    }
}
