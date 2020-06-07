using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarbonCalculator
{
    public class CementVM : ViewModelBase, IViewModelParent
    {
        CarbonMaterials.ICE3CementModel _cement;

        List<string> _allConstituents;
        public List<string> AllConstituents => _allConstituents;

        public double TotalProportion
        {
            get
            {
                return _cement.Constituents.Sum(a => a.Proportion);
            }
        }

        public double TotalCarbon
        {
            get
            {
                return _cement.Constituents.Sum(a => a.Proportion * a.Material.EmbodiedCarbon);
            }
        }

        public CementVM(CarbonMaterials.ICE3CementModel cement)
        {
            _cement = cement;
            _allConstituents = _cement.AllConstituents.Keys.ToList();
        }

        ObservableCollection<ConstituentVM> _constituents;
        public ObservableCollection<ConstituentVM> Constituents
        {
            get
            {
                if (_constituents == null)
                {
                    _constituents = new ObservableCollection<ConstituentVM>();
                    foreach (var item in _cement.Constituents)
                    {
                        _constituents.Add(new ConstituentVM(item, this));
                    }
                }
                return _constituents;
            }
        }

        ICommand _addConstituentCommand;

        public ICommand AddConstituentCommand
        {
            get
            {
                return _addConstituentCommand ?? (_addConstituentCommand = new CommandHandler(() => addConstituent(), true));
            }
        }

        void addConstituent()
        {
            var constit = new CarbonMaterials.CementAndConcreteConstituent
            {
                Material = _cement.AllConstituents["Clinker"],
                Proportion = 0
            };
            _cement.Constituents.Add(constit);
            _constituents.Add(new ConstituentVM(constit, this));
            RaisePropertyChanged(nameof(TotalCarbon));
            RaisePropertyChanged(nameof(TotalProportion));
        }

        public void UpdateAll()
        {
            RaisePropertyChanged(nameof(TotalCarbon));
            RaisePropertyChanged(nameof(TotalProportion));
        }

        bool _accepted = false;
        public bool Accepted
        {
            get
            {
                return _accepted;
            }
        }

        ICommand _acceptAndCloseCommand;

        public ICommand AcceptAndCloseCommand
        {
            get
            {
                return _acceptAndCloseCommand ?? (_acceptAndCloseCommand = new CommandHandlerWithParameter(new Action<object>(acceptAndClose), true));
            }
        }

        private void acceptAndClose(object obj)
        {
            var win = obj as Window;
            _accepted = true;
            win.Close();
        }

        ICommand _cancelAndCloseCommand;

        public ICommand CancelAndCloseCommand
        {
            get
            {
                return _cancelAndCloseCommand ?? (_cancelAndCloseCommand = new CommandHandlerWithParameter(new Action<object>(cancelAndClose), true));
            }
        }

        private void cancelAndClose(object obj)
        {
            var win = obj as Window;
            _accepted = false;
            win.Close();
        }
    }
}
