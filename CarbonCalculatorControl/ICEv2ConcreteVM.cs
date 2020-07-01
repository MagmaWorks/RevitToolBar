using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CarbonMaterials;

namespace CarbonCalculator
{
    public class ICEv2ConcreteVM : ICEMaterialVMBase, IViewModelParent
    {
        ICEConcrete _concMat;

        public ICEv2ConcreteVM()
        {

        }

        public ICEv2ConcreteVM(ICEConcrete material, Measurement measure)
        {
            _measure = measure;

            _material = material;
            _concMat = material;
            
            setTransportToSite();
            setTransportToDisposal();
        }

        public string ConcreteGrade
        {
            get
            {
                if (_material is ICEConcrete)
                    return (_material as ICEConcrete).Grade;
                else
                    return "";
            }
            set
            {
                (_material as ICEConcrete).Grade = value;
                RaisePropertyChanged(nameof(TotalCarbon));
                RaisePropertyChanged(nameof(ConcreteGrade));
                RaisePropertyChanged(nameof(A1toA3));
                RaisePropertyChanged(nameof(Name));


            }
        }

        public string ConcreteReplacement
        {
            get
            { 
                return _concMat.Replacement;

            }
            set
            {
                _concMat.Replacement = value;
                RaisePropertyChanged(nameof(TotalCarbon));
                RaisePropertyChanged(nameof(ConcreteReplacement));
                RaisePropertyChanged(nameof(A1toA3));
                RaisePropertyChanged(nameof(Name));

            }
        }

        public double ConcreteReinforcementDensity
        {
            get
            {
                return _concMat.ReinforcementDensity;           
            }
            set
            {
                _concMat.ReinforcementDensity = value;
                RaisePropertyChanged(nameof(TotalCarbon));
                RaisePropertyChanged(nameof(ConcreteReinforcementDensity));
                RaisePropertyChanged(nameof(A1toA3));
                RaisePropertyChanged(nameof(Name));
            }
        }

        public List<string> Replacements
        {
            get
            {
                return ICEConcrete.ConcreteReplacements;
            }
        }

        public List<string> Grades
        {
            get
            {
                return ICEConcrete.ConcreteGrades;
            }
        }
    }
}
