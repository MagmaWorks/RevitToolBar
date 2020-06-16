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

        int _selectedMaterialIndex = 0;
        public int SelectedMaterialIndex
        {
            get
            {
                return _selectedMaterialIndex;
            }
            set
            {
                _selectedMaterialIndex = value;
                RaisePropertyChanged(nameof(SelectedMaterialIndex));
                RaisePropertyChanged(nameof(SelectedMaterial));
            }
        }

        public MaterialVM SelectedMaterial
        {
            get
            {
                if (Materials.Count > 0)
                    return Materials[Math.Max(_selectedMaterialIndex, 0)];
                else
                    return null;
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
        public ObservableCollection<string> MaterialNames { get; }


        public MaterialSetVM(string name, GWPMaterialSet materialSet, IViewModelParent parent)
        {
            _materials = materialSet;
            _parent = parent;
            _materialVMs = new ObservableCollection<MaterialVM>();
            MaterialNames = new ObservableCollection<string>();
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
            MaterialNames = new ObservableCollection<string>();
            foreach (var item in materialSet.Materials)
            {
                MaterialNames.Add(item.Name);
                _materialVMs.Add(new MaterialVM(item, this));
            }

        }

        //public void deleteItem(MaterialVM mat)
        //{
        //    if (_materials.Materials.Count > 0)
        //    {
        //        _materials.Materials.Remove(mat.Material);
        //        _materialVMs.Remove(mat);
        //        if (_selectedMaterialIndex >= Materials.Count)
        //        {
        //            _selectedMaterialIndex = Materials.Count - 1;
        //        }
        //    }
        //    _parent.UpdateAll();
        //}


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
            MaterialNames.Add(newMat.Name);
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
            MaterialNames.Add(newMat.Name);
            _parent.UpdateAll();

        }

        ICommand _addGeneralCommand;

        public ICommand AddGeneralCommand
        {
            get
            {
                return _addGeneralCommand ?? (_addGeneralCommand = new CommandHandler(() => addGeneral(), true));
            }
        }

        void addGeneral()
        {
            var newMat = new ICEv3General();
            _materials.Materials.Add(newMat);
            _materialVMs.Add(new MaterialVM(newMat, this));
            MaterialNames.Add(newMat.Name);
            _parent.UpdateAll();

        }

        ICommand _addGeneralv2Command;

        public ICommand AddGeneralv2Command
        {
            get
            {
                return _addGeneralv2Command ?? (_addGeneralv2Command = new CommandHandler(() => addGeneralv2(), true));
            }
        }

        void addGeneralv2()
        {
            var newMat = new ICEv2General();
            _materials.Materials.Add(newMat);
            _materialVMs.Add(new MaterialVM(newMat, this));
            MaterialNames.Add(newMat.Name);
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
            MaterialNames.Add(newMat.Name);
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
            MaterialNames.Add(newMat.Name);
            _parent.UpdateAll();

        }

        ICommand _addConcreteV3Command;

        public ICommand AddConcreteV3Command
        {
            get
            {
                return _addConcreteV3Command ?? (_addConcreteV3Command = new CommandHandler(() => addConcreteV3(), true));
            }
        }

        void addConcreteV3()
        {
            var newMat = new ICE3ConcreteModel();
            _materials.Materials.Add(newMat);
            _materialVMs.Add(new MaterialVM(newMat, this));
            MaterialNames.Add(newMat.Name);
            _parent.UpdateAll();

        }

    }
}
