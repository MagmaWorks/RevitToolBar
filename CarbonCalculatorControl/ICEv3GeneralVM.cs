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
        ICEv3General _genMat;

        List<ICEv3General.ICEv3DBMaterial> generalMaterials;

        List<string> _materialNames;
        public List<string> MaterialNames
        {
            get
            {            
                return _materialNames;
            }
        }

        string _selectedMaterialName = "";
        public string SelectedMaterialName
        {
            get
            {
                return _selectedMaterialName;
            }
            set
            {
                _selectedMaterialName = value;
                _subMaterialNames = generalMaterials
                    .Where(a => a.Material == _selectedMaterialName)
                    .ToList();
                RaisePropertyChanged(nameof(SelectedMaterialName));
                RaisePropertyChanged(nameof(SubMaterialNames));
                SelectedSubMaterialName = _subMaterialNames[0];
                RaisePropertyChanged(nameof(ICEv3Notes));
                RaisePropertyChanged(nameof(A1toA3));
                UpdateAll();
            }
        }

        List<ICEv3General.ICEv3DBMaterial> _subMaterialNames = new List<ICEv3General.ICEv3DBMaterial>();
        public List<ICEv3General.ICEv3DBMaterial> SubMaterialNames
        {
            get
            {
                return _subMaterialNames;
            }
        }

        ICEv3General.ICEv3DBMaterial _selectedSubMaterialName;
        public ICEv3General.ICEv3DBMaterial SelectedSubMaterialName
        {
            get
            {
                return _selectedSubMaterialName;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                _selectedSubMaterialName = value;
                RaisePropertyChanged(nameof(SelectedSubMaterialName));
                _genMat.UpdateValues(_selectedSubMaterialName.Material, _selectedSubMaterialName.SubMaterial, _selectedSubMaterialName.EmbodiedCarbonPerMass, _selectedSubMaterialName.SequesteredCarbon, _selectedSubMaterialName.MaterialDensity, _selectedSubMaterialName.GUID);
                ICEv3Notes = _selectedSubMaterialName.Comments;
                RaisePropertyChanged(nameof(ICEv3Notes));
                RaisePropertyChanged(nameof(A1toA3));
                UpdateAll();
            }
        }

        new public double MassDensity
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

        public string ICEv3Notes { get; private set; }

        List<CarbonMaterials.ICEv3General.ICEv3DBMaterial> _materials;
        public List<CarbonMaterials.ICEv3General.ICEv3DBMaterial> Materials
        {
            get
            {
                if (_materials != null)
                {
                    return _materials;
                }
                else
                {
                    _materials = ICEv3General.ReadICEv3Materials();
                    return _materials;
                }
            }
        }

        public ICEv3GeneralVM(ICEv3General material, Measurement measure)
        {
            _measure = measure;

            generalMaterials = ICEv3General.ReadICEv3Materials();
            _materialNames = new List<string>(generalMaterials.Select(a => a.Material).Distinct());

            _genMat = material;
            _material = material;

            _selectedSubMaterialName = generalMaterials.Where(a => a.GUID == material.Guid).FirstOrDefault();

            if (_selectedSubMaterialName != null)
            {
                _selectedMaterialName = _selectedSubMaterialName.Material;
                _subMaterialNames = generalMaterials
                    .Where(a => a.Material == _selectedMaterialName)
                    .ToList();
            }
            else
            {
                _selectedMaterialName = _materialNames[0];
                _subMaterialNames = generalMaterials
                    .Where(a => a.Material == _selectedMaterialName)
                    .ToList();
            }

            ICEv3Notes = _selectedSubMaterialName.Comments;

            setTransportToSite();
            setTransportToDisposal();
        }    
    }
}
