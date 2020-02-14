using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CarbonMaterials;

namespace CarbonCalculator
{
    public class MaterialSetVM : ViewModelBase
    {
        GWPMaterialSet _materials;
        public GWPMaterialSet GWPMaterial => _materials;

        ObservableCollection<MaterialVM> _materialVMs;
        public ObservableCollection<MaterialVM> Materials
        {
            get
            {
                return _materialVMs;
            }
        }
        ModelVM _parent;
        public string Name => _materials.Name;
        public List<string> MaterialNames { get; }

        bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
            }
        }

        public MaterialSetVM(string name, GWPMaterialSet materialSet, ModelVM parent)
        {
            _materials = materialSet;
            _parent = parent;
            _materialVMs = new ObservableCollection<MaterialVM>();
            MaterialNames = new List<string>();
            foreach (var item in materialSet.Materials)
            {
                MaterialNames.Add(item.Name);
                _materialVMs.Add(new MaterialVM(item));
            }

        }

        ICommand _addConcreteCommand;

        public ICommand AddConcreteCommand
        {
            get
            {
                return _addConcreteCommand ?? (_addConcreteCommand = new CommandHandler(() => addConcrete(),true));
            }
        }

        void addConcrete()
        {
            var newMat = new ICEConcrete();
            _materials.Materials.Add(newMat);
            _materialVMs.Add(new MaterialVM(newMat));
        }

        ICommand _addSteelCommand;

        public ICommand AddSteelCommand
        {
            get
            {
                return _addSteelCommand ?? (_addSteelCommand = new CommandHandler(() => addSteel(), true));
            }
        }

        void addSteel()
        {
            var newMat = new ICESteel();
            _materials.Materials.Add(newMat);
            _materialVMs.Add(new MaterialVM(newMat));
        }

        ICommand _addTimberCommand;

        public ICommand AddTimberCommand
        {
            get
            {
                return _addTimberCommand ?? (_addTimberCommand = new CommandHandler(() => addTimber(), true));
            }
        }

        void addTimber()
        {
            var newMat = new ICETimber();
            _materials.Materials.Add(newMat);
            _materialVMs.Add(new MaterialVM(newMat));
        }

    }
}
