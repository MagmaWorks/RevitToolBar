using CarbonMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarbonCalculator
{
    public class MaterialVM : ViewModelBase
    {
        GWPMaterial _material;
        public GWPMaterial Material
        {
            get
            {
                return _material;
            }
        }

        MaterialSetVM _parent;

        public string Name => _material.Name;

        public MaterialVM(GWPMaterial material, MaterialSetVM parent)
        {
            _material = material;
            _parent = parent;
        }

        ICommand _editMaterialCommand;

        public ICommand EditMaterialCommand
        {
            get
            {
                return _editMaterialCommand ?? (_editMaterialCommand = new CommandHandler(() => editMaterial(), true));
            }
        }
         
        void editMaterial()
        {
            if (_material.GWPMaterialType == GWPMaterialType.ICE)
            {
                var editedMaterial = _material.getCopy() as ICEMaterial;
                var materialEditorVM = new ICEMaterialVM((editedMaterial));
                Window myWindow = new Window()
                {
                    Content = new MaterialEditor(),
                    DataContext = materialEditorVM,
                    Width = 800,
                    Height = 900
                };
                myWindow.ShowDialog();
                if (materialEditorVM.Accepted)
                {
                    _material.copyMaterialFrom(editedMaterial);
                    RaisePropertyChanged("");
                }
            }
            else
            {
                var editedMaterial = _material.getCopy() as GWPGeneric;
                var materialEditorVM = new GWPGenericMaterialVM((editedMaterial));
                Window myWindow = new Window()
                {
                    Content = new EPDMaterialEditor(),
                    DataContext = materialEditorVM,
                    Width = 800,
                    Height = 300
                };
                myWindow.ShowDialog();
                if (materialEditorVM.Accepted)
                {
                    _material.copyMaterialFrom(editedMaterial);
                    RaisePropertyChanged("");
                }
            }

            RaisePropertyChanged(nameof(Name));
            _parent.Parent.UpdateAll();

        }

        ICommand _deleteMaterialCommand;

        public ICommand DeleteMaterialCommand
        {
            get
            {
                return _deleteMaterialCommand ?? (_deleteMaterialCommand = new CommandHandler(() => deleteMaterial(), true));
            }
        }

        void deleteMaterial()
        {
            _parent.deleteItem(this);
            _parent.Parent.UpdateAll();
        }
    }
}
