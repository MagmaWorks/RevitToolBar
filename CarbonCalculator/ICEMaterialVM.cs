using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CarbonMaterials;

namespace CarbonCalculator
{
    public class ICEMaterialVM : ViewModelBase
    {
        ICEMaterial _material;

        public double MassDensity
        {
            get
            {
                return _material.MassDensity;
            }
            set
            {
                _material.ChangeMassDensity(value);
                RaisePropertyChanged(nameof(MassDensity));
            }
        }          

        public ICECategory Category
        {
            get
            {
                return _material.Category;
            }
        }

        public string Name => _material.Name;

        public double A1toA3 => _material.A1toA3.Value;

        public ICEMaterialVM()
        {
            //material = new ICEConcrete("RC32/40", "25%GGBS", 150);
            _material = new ICETimber("Sawn Softwood", true);
            _material.TransportsToSite.Add(new MaterialTransport());
            _material.TransportsToSite.Add(new MaterialTransport());
            _material.TransportsToDispoal.Add(new MaterialTransport());
            _material.TransportsToDispoal.Add(new MaterialTransport());
            setTransportToSite();
            setTransportToDisposal();
        }

        public ICEMaterialVM(ICEMaterial material)
        {
            _material = material;
            setTransportToSite();
            setTransportToDisposal();
        }

        public void Update()
        {
            RaisePropertyChanged(nameof(A4));
            RaisePropertyChanged(nameof(C2));
            RaisePropertyChanged(nameof(TotalCarbon));

        }

        void setTransportToSite()
        {
            _transportToSiteDefinitions = new ObservableCollection<TransportVM>();
            foreach (var item in _material.TransportsToSite)
            {
                _transportToSiteDefinitions.Add(new TransportVM(item, this));
            }
        }

        void setTransportToDisposal()
        {
            _transportToDisposalDefinitions = new ObservableCollection<TransportVM>();
            foreach (var item in _material.TransportsToDispoal)
            {
                _transportToDisposalDefinitions.Add(new TransportVM(item, this));
            }
        }

        ObservableCollection<TransportVM> _transportToSiteDefinitions;
        public ObservableCollection<TransportVM> TransportToSiteDefinitions
        {
            get
            {
                return _transportToSiteDefinitions;
            }
        }

        public double A4
        {
            get
            {
                return _material.A4.Value;
            }
        }

        public double ConstructionFactor
        {
            get
            {
                return _material.ConstructionFactor;
            }
            set
            {
                _material.ConstructionFactor = value;
                RaisePropertyChanged(nameof(A5));
                RaisePropertyChanged(nameof(TotalCarbon));
            }
        }

        public double A5
        {
            get
            {
                return _material.A5.Value;
            }
        }

        public double InUseFactor
        {
            get
            {
                return _material.InUseFactor;
            }
            set
            {
                _material.InUseFactor = value;
                RaisePropertyChanged(nameof(B1));
                RaisePropertyChanged(nameof(TotalCarbon));
            }
        }

        public double B1
        {
            get
            {
                return _material.B1.Value;
            }
        }

        public double B2 => _material.B2.Value;
        public double B3 => _material.B3.Value;
        public double B4 => _material.B4.Value;
        public double B5 => _material.B5.Value;
        public double B6 => _material.B6.Value;
        public double B7 => _material.B7.Value;

        public double DeConstructionFactor
        {
            get
            {
                return _material.DeConstructionFactor;
            }
            set
            {
                _material.DeConstructionFactor = value;
                RaisePropertyChanged(nameof(C1));
                RaisePropertyChanged(nameof(TotalCarbon));
            }
        }

        public double C1
        {
            get
            {
                return _material.C1.Value;
            }
        }

        ObservableCollection<TransportVM> _transportToDisposalDefinitions;
        public ObservableCollection<TransportVM> TransportToDisposalDefinitions
        {
            get
            {
                return _transportToDisposalDefinitions;
            }
        }

        public double C2
        {
            get
            {
                return _material.C2.Value;
            }
        }

        public double RecyclingReuseFactor
        {
            get
            {
                return _material.RecyclingReuseFactor;
            }
            set
            {
                _material.RecyclingReuseFactor = value;
                RaisePropertyChanged(nameof(C3));
                RaisePropertyChanged(nameof(TotalCarbon));
            }
        }

        public double C3
        {
            get
            {
                return _material.C3.Value;
            }
        }

        public double C4
        {
            get
            {
                return _material.C4.Value;
            }
        }

        public string TotalCarbon
        {
            get
            {
                return string.Format("{0:0.00}", _material.TotalAtoC) + "kg / m3";
            }
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
                if (_material is ICEConcrete)
                    return (_material as ICEConcrete).Replacement;
                else
                    return "";
            }
            set
            {
                (_material as ICEConcrete).Replacement = value;
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
                if (_material is ICEConcrete)
                    return (_material as ICEConcrete).ReinforcementDensity;
                else
                    return 0;                
            }
            set
            {
                (_material as ICEConcrete).ReinforcementDensity = value;
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

        public bool IncludeCarbonFromBiomass
        {
            get
            {
                if (_material is ICETimber)
                    return (_material as ICETimber).IncludeCarbonFromBiomass;
                else
                    return false;
            }
            set
            {
                (_material as ICETimber).IncludeCarbonFromBiomass = value;
                RaisePropertyChanged(nameof(TotalCarbon));
                RaisePropertyChanged(nameof(IncludeCarbonFromBiomass));
                RaisePropertyChanged(nameof(A1toA3));
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string TimberMaterial
        {
            get
            {
                if (_material is ICETimber)
                    return (_material as ICETimber).TimberMaterial;
                else
                    return "";                
            }
            set
            {
                if (_material is ICETimber)
                    (_material as ICETimber).TimberMaterial = value;
                RaisePropertyChanged(nameof(TotalCarbon));
                RaisePropertyChanged(nameof(TimberMaterial));
                RaisePropertyChanged(nameof(A1toA3));
                RaisePropertyChanged(nameof(Name));
            }
        }

        public List<string> Materials
        {
            get
            {
                if (_material is ICETimber)
                    return (_material as ICETimber).Materials;
                else
                    return new List<string> { "" };
            }
        }

        public List<string> SteelMaterials
        {
            get
            {
                if (_material is ICESteel)
                    return (_material as ICESteel).SteelMaterials;
                else
                    return new List<string> { "" };
            }
        }

        public string SteelMaterial
        {
            get
            {
                if (_material is ICESteel)
                    return (_material as ICESteel).SteelMaterial;
                else
                    return "";
            }
            set
            {
                if (_material is ICESteel)
                    (_material as ICESteel).SteelMaterial = value;
                RaisePropertyChanged(nameof(TotalCarbon));
                RaisePropertyChanged(nameof(SteelMaterial));
                RaisePropertyChanged(nameof(A1toA3));
                RaisePropertyChanged(nameof(Name));
            }
        }

        ICommand _addTransportToSiteCommand;

        public ICommand AddTransportToSiteCommand
        {
            get
            {
                return _addTransportToSiteCommand ?? (_addTransportToSiteCommand = new CommandHandler(() => addTransportToSite(), true));
            }
        }

        void addTransportToSite()
        {
            var matTrans = new MaterialTransport();
            _material.TransportsToSite.Add(matTrans);
            TransportToSiteDefinitions.Add(new TransportVM(matTrans, this));
        }

        ICommand _addTransportToDisposalCommand;

        public ICommand AddTransportToDisposalCommand
        {
            get
            {
                return _addTransportToDisposalCommand ?? (_addTransportToDisposalCommand = new CommandHandler(() => addTransportToDisposal(), true));
            }
        }

        void addTransportToDisposal()
        {
            var matTrans = new MaterialTransport();
            _material.TransportsToDispoal.Add(matTrans);
            TransportToDisposalDefinitions.Add(new TransportVM(matTrans, this));

        }
    }


}
