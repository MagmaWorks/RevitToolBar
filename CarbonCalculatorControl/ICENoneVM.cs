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
    public class ICENoneVM : ICEMaterialVMBase, IViewModelParent
    {
        public ICENoneVM()
        {

        }

        public ICENoneVM(ICENone material)
        {
            _material = material;
            
            setTransportToSite();
            setTransportToDisposal();
        }    
    }
}
