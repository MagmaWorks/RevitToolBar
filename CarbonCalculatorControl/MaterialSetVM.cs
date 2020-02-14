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
        IViewModelParent _parent;
        public IViewModelParent Parent
        {
            get
            {
                return _parent;
            }
        }
        public string Name
        {
            get
            {
                return _materials.Name;
            }
            set
            {
                _materials.Name = value;
                RaisePropertyChanged(nameof(Name));
                _parent.UpdateAll();
            }
        }
        public List<string> MaterialNames { get; }


        public MaterialSetVM(string name, GWPMaterialSet materialSet, IViewModelParent parent)
        {
            _materials = materialSet;
            _parent = parent;
            _materialVMs = new ObservableCollection<MaterialVM>();
            MaterialNames = new List<string>();
            foreach (var item in materialSet.Materials)
            {
                MaterialNames.Add(item.Name);
                _materialVMs.Add(new MaterialVM(item, this));
            }

        }


        public MaterialSetVM(GWPMaterialSet materialSet, IViewModelParent parent)
        {
            _materials = materialSet;
            _parent = parent;
            _materialVMs = new ObservableCollection<MaterialVM>();
            MaterialNames = new List<string>();
            foreach (var item in materialSet.Materials)
            {
                MaterialNames.Add(item.Name);
                _materialVMs.Add(new MaterialVM(item, this));
            }

        }

        public void deleteItem(MaterialVM mat)
        {
            if (_materials.Materials.Count > 0)
            {
                _materials.Materials.Remove(mat.Material);
                _materialVMs.Remove(mat);
            }
            _parent.UpdateAll();
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
            _materialVMs.Add(new MaterialVM(newMat, this));
            _parent.UpdateAll();

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
            _materialVMs.Add(new MaterialVM(newMat, this));
            _parent.UpdateAll();

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
            _materialVMs.Add(new MaterialVM(newMat, this));
            _parent.UpdateAll();

        }

        ICommand _addGeneric;

        public ICommand AddGeneric
        {
            get
            {
                return _addGeneric ?? (_addGeneric = new CommandHandler(() => addGeneric(), true));
            }
        }

        void addGeneric()
        {
            var newMat = new GWPGeneric();
            _materials.Materials.Add(newMat);
            _materialVMs.Add(new MaterialVM(newMat, this));
            _parent.UpdateAll();

        }

    }
}
