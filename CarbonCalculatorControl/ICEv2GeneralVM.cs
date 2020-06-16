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
    public class ICEv2GeneralVM : ICEMaterialVMBase, IViewModelParent
    {
        ICEv2General _genMat;

        ICEv2General.ICEv2DBMaterial _selectedMaterial;
        public ICEv2General.ICEv2DBMaterial SelectedMaterial
        {
            get
            {
                return _selectedMaterial;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                _selectedMaterial = value;
                RaisePropertyChanged(nameof(SelectedMaterial));
                _genMat.UpdateValues(_selectedMaterial.MaterialFamily, _selectedMaterial.Material, _selectedMaterial.EmbodiedCarbon, _selectedMaterial.Reference);
                ICEv2Notes = _selectedMaterial.Notes;
                RaisePropertyChanged(nameof(ICEv2Notes));
                RaisePropertyChanged(nameof(A1toA3));
                UpdateAll();
            }
        }

        public string ICEv2Notes { get; private set; }

        List<CarbonMaterials.ICEv2General.ICEv2DBMaterial> _materials;
        public List<CarbonMaterials.ICEv2General.ICEv2DBMaterial> Materials
        {
            get
            {
                if (_materials != null)
                {
                    return _materials;
                }
                else
                {
                    _materials = ICEv2General.ReadICEv2Materials();
                    return _materials;
                }
            }
        }

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
                UpdateAll();
            }
        }

        public ICEv2GeneralVM(ICEv2General material)
        {
            _materials = ICEv2General.ReadICEv2Materials();
            _selectedMaterial = _materials.Where(a => a.Reference == material.Reference).FirstOrDefault();
            _genMat = material;
            _material = material;
            RaisePropertyChanged(nameof(Materials));

            _genMat.UpdateValues(_selectedMaterial.MaterialFamily, _selectedMaterial.Material, _selectedMaterial.EmbodiedCarbon, _selectedMaterial.Reference);
            
            setTransportToSite();
            setTransportToDisposal();
        }    
    }
}
