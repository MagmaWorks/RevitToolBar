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
    public class ICEv3ConcreteVM : ICEMaterialVMBase, IViewModelParent
    {
        CarbonMaterials.ICE3ConcreteModel _concMat;

        public ICEv3ConcreteVM()
        {

        }

        public ICEv3ConcreteVM(ICE3ConcreteModel material, Measurement measure)
        {
            _measure = measure;

            _material = material;
            _concMat = material;
            
            setTransportToSite();
            setTransportToDisposal();
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

        ObservableCollection<ConstituentVM> _constituents;
        public ObservableCollection<ConstituentVM> Constituents
        {
            get
            {
                if (!(_material is ICE3ConcreteModel))
                {
                    return null;
                }
                if (_constituents == null)
                {
                    _constituents = new ObservableCollection<ConstituentVM>();
                    foreach (var item in (_material as ICE3ConcreteModel).Constituents)
                    {
                        _constituents.Add(new ConstituentVM(item, this));
                    }
                }
                return _constituents;
            }
        }

        ICommand _addConstituentCommand;

        public ICommand AddConstituentCommand
        {
            get
            {
                return _addConstituentCommand ?? (_addConstituentCommand = new CommandHandler(() => addConstituent(), true));
            }
        }

        void addConstituent()
        {
            var constit = new CarbonMaterials.CementAndConcreteConstituent
            {
                Material = ICE3CementModel.getCementConstituents()["Aggregates"],
                Proportion = 0
            };
            (_material as CarbonMaterials.ICE3ConcreteModel).Constituents.Add(constit);
            Constituents.Add(new ConstituentVM(constit, this));
        }


    }
}
