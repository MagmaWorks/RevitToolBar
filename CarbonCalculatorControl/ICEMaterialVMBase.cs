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
    public class ICEMaterialVMBase : ViewModelBase, IViewModelParent
    {
        protected ICEMaterial _material;

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
                Update();
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

        public void Update()
        {
            RaisePropertyChanged(nameof(A4));
            RaisePropertyChanged(nameof(C2));
            RaisePropertyChanged(nameof(TotalCarbon));
        }

        public void UpdateAll()
        {
            RaisePropertyChanged("");
        }

        protected void setTransportToSite()
        {
            _transportToSiteDefinitions = new ObservableCollection<TransportVM>();
            foreach (var item in _material.TransportsToSite)
            {
                _transportToSiteDefinitions.Add(new TransportVM(item, this));
            }
        }

        protected void setTransportToDisposal()
        {
            _transportToDisposalDefinitions = new ObservableCollection<TransportVM>();
            foreach (var item in _material.TransportsToDispoal)
            {
                _transportToDisposalDefinitions.Add(new TransportVM(item, this));
            }
        }

        ObservableCollection<TransportVM> _transportToSiteDefinitions = new ObservableCollection<TransportVM>();
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

        ObservableCollection<TransportVM> _transportToDisposalDefinitions = new ObservableCollection<TransportVM>();
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

        public string SequesteredCarbon
        {
            get
            {
                return string.Format("{0:0.00}", _material.SequesteredCarbonDensity + "kg / m3");
            }
        }

        public string SequesteredCarbonComment
        {
            get
            {
                if (_material.IncludeSequesteredCarbon)
                    return "Sequestered carbon is included within figures. Total removed from A1 and added to C3.";
                else
                    return "Sequestration of carbon is not taken into account.";

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
