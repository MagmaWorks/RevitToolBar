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
    public class ICEv3GeneralVM : ICEMaterialVMBase, IViewModelParent
    {
        ICEGeneral _genMat;

        public ICEv3GeneralVM()
        {

        }

        public ICEv3GeneralVM(ICEGeneral material)
        {
            _material = material;
            _genMat = material;
            
            setTransportToSite();
            setTransportToDisposal();
        }    
    }
}
