using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WhitbyWoodToolbar
{
    public class ElementCarbonVM : ViewModelBase
    {
        double _volume;
        public double Volume
        {
            get
            {
                return _volume;
            }
        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string LongName
        {
            get
            {
                return _category.Name + ": " + _name + ": " + _revitMaterial.Name;
            }
        }

        RevitCategory _category;
        public RevitCategory Category
        {
            get
            {
                return _category;
            }
        }

        RevitMaterial _revitMaterial;
        public RevitMaterial RevitMaterial
        {
            get
            {
                return _revitMaterial;
            }
        }

        public double EmbodiedCarbon
        {
            get
            {
                return _volume * ICEMaterial.CarbonDensity;
            }
        }

        bool _elementSelected = true;
        public bool ElementSelected
        {
            get
            {
                return _elementSelected;
            }
            set
            {
                _elementSelected = value;
                RaisePropertyChanged(nameof(ElementSelected));
            }
        }

        public bool IsSelected
        {
            get
            {
                return (_revitMaterial.IsSelected && _category.IsSelected);
            }
        }

        public void selectionUpdated()
        {
            RaisePropertyChanged(nameof(IsSelected));
        }

        MaterialsVM _parent;

        ICEMaterial _iceMat;
        public ICEMaterial ICEMaterial
        {
            get
            {
                return _iceMat;
            }
        }

        public string MaterialType
        {
            get
            {
                return ICEMaterial.Category.ToString();
            }
            set
            {
                var newMat = ICEMaterial.CreateMaterial(value, this);
                _iceMat = newMat;
                RaisePropertyChanged(nameof(ICEMaterial));
                RaisePropertyChanged(nameof(MaterialType));
                RaisePropertyChanged(nameof(Category));
                RaisePropertyChanged(nameof(EmbodiedCarbon));
                _parent.Updated();
            }
        }

        public List<string> MaterialTypes
        {
            get
            {
                return _parent.MaterialTypes;
            }
        }

        public void UpdateICEMaterial(ICEMaterial mat)
        {
            _iceMat = mat;
            RaisePropertyChanged(nameof(ICEMaterial));
            RaisePropertyChanged(nameof(MaterialType));
            RaisePropertyChanged(nameof(Category));
            RaisePropertyChanged(nameof(EmbodiedCarbon));
        }

        public void ICEMaterialUpdated()
        {
            RaisePropertyChanged(nameof(MaterialType));
            RaisePropertyChanged(nameof(Category));
            RaisePropertyChanged(nameof(EmbodiedCarbon));
            _parent.Updated();
        }

        public ElementCarbonVM(string name, double vol, RevitMaterial mat, RevitCategory cat, MaterialsVM parent)
        {
            _name = name;
            _volume = vol;
            _revitMaterial = mat;
            _category = cat;
            _parent = parent;
            _iceMat = new ICEMaterialNone(this);
        }

        public override string ToString()
        {
            return _category.Name + ": " + _name + ": " + _revitMaterial.Name;
        }

        ICommand _copyMaterialCommand;

        public ICommand CopyMaterialCommand
        {
            get
            {
                return _copyMaterialCommand ?? (_copyMaterialCommand = new CommandHandler(new Action(copyMaterials), true));
            }
        }

        void copyMaterials()
        {
            _parent.ChangeICEMaterial(ICEMaterial);
        }
    }
}
