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
    public class ICEv2SteelVM : ICEMaterialVMBase, IViewModelParent
    {
        ICESteel _concMat;

        public ICEv2SteelVM()
        {

        }

        public ICEv2SteelVM(ICESteel material, Measurement measure)
        {
            _measure = measure;

            _material = material;
            _concMat = material;
            
            setTransportToSite();
            setTransportToDisposal();
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
    }
}
