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
    public class ICEv2TimberVM : ICEMaterialVMBase, IViewModelParent
    {
        ICETimber _concMat;

        public ICEv2TimberVM()
        {

        }

        public ICEv2TimberVM(ICETimber material)
        {
            _material = material;
            _concMat = material;
            
            setTransportToSite();
            setTransportToDisposal();
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
    }
}
