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

        public string Name => _material.Name;

        public MaterialVM(GWPMaterial material)
        {
            _material = material;
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
            var check = _material as ICEMaterial;
            var materialEditorVM = new ICEMaterialVM((_material as ICEMaterial));
            Window myWindow = new Window()
            {
                Content = new MaterialEditor(),
                DataContext = materialEditorVM
            };
            myWindow.ShowDialog();
            RaisePropertyChanged(nameof(Name));
        }
    }
}
