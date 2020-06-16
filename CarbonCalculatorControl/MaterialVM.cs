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

        public MaterialVM(GWPMaterial material)
        {
            _material = material;
            _parent = null;
        }

        //ICommand _editMaterialCommand;

        //public ICommand EditMaterialCommand
        //{
        //    get
        //    {
        //        return _editMaterialCommand ?? (_editMaterialCommand = new CommandHandler(() => editMaterial(), true));
        //    }
        //}

        public object VM
        {
            get
            {
                if (_material.GWPMaterialType == GWPMaterialType.ICE)
                {
                    return getICEVM(_material as ICEMaterial);
                }
                else
                {
                    return new GWPGenericMaterialVM(_material as CarbonMaterials.GWPGeneric);
                }
            }
        }

        ICEMaterialVMBase getICEVM(ICEMaterial mat)
        {
            switch (mat)
            {
                case ICEConcrete m:
                    return new ICEv2ConcreteVM(mat as ICEConcrete);
                case ICESteel m:
                    return new ICEv2SteelVM(mat as ICESteel);
                case ICETimber m:
                    return new ICEv2TimberVM(mat as ICETimber);
                case ICE3ConcreteModel m:
                    return new ICEv3ConcreteVM(mat as ICE3ConcreteModel);
                case ICEv3General m:
                    return new ICEv3GeneralVM(mat as ICEv3General);
                case ICENone m:
                    return new ICENoneVM(mat as ICENone);
                case ICEv2General m:
                    return new ICEv2GeneralVM(mat as ICEv2General);
                default:
                    return new ICEMaterialVMBase();
            }
        }
         
        //void editMaterial()
        //{
        //    if (_material.GWPMaterialType == GWPMaterialType.ICE)
        //    {
        //        var editedMaterial = _material.getCopy() as ICEMaterial;
        //        var materialEditorVM = new ICEMaterialVM((editedMaterial));
        //        Window myWindow = new Window()
        //        {
        //            Content = new MaterialEditor(),
        //            DataContext = materialEditorVM,
        //            Width = 800,
        //            Height = 900
        //        };
        //        myWindow.ShowDialog();
        //        if (materialEditorVM.Accepted)
        //        {
        //            _material.copyMaterialFrom(editedMaterial);
        //            RaisePropertyChanged("");
        //        }
        //    }
        //    else
        //    {
        //        var editedMaterial = _material.getCopy() as GWPGeneric;
        //        var materialEditorVM = new GWPGenericMaterialVM((editedMaterial));
        //        Window myWindow = new Window()
        //        {
        //            Content = new EPDMaterialEditor(),
        //            DataContext = materialEditorVM,
        //            Width = 800,
        //            Height = 300
        //        };
        //        myWindow.ShowDialog();
        //        if (materialEditorVM.Accepted)
        //        {
        //            _material.copyMaterialFrom(editedMaterial);
        //            RaisePropertyChanged("");
        //        }
        //    }

        //    RaisePropertyChanged(nameof(Name));
        //    _parent.Parent.UpdateAll();

        //}

        //ICommand _deleteMaterialCommand;

        //public ICommand DeleteMaterialCommand
        //{
        //    get
        //    {
        //        return _deleteMaterialCommand ?? (_deleteMaterialCommand = new CommandHandler(() => deleteMaterial(), true));
        //    }
        //}

        //void deleteMaterial()
        //{
        //    _parent.deleteItem(this);
        //    _parent.Parent.UpdateAll();
        //}
    }
}
