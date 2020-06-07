using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarbonCalculator
{
    public class ConstituentVM : ViewModelBase
    {
        IViewModelParent _parent;

        CarbonMaterials.CementAndConcreteConstituent _constituent;

        public List<string> ConstituentNames
        {
            get
            {
                return _constituent.Material.AllConstituents.Keys.ToList();
            }
        }

        public string Name
        {
            get
            {
                return _constituent.Material.Name;
            }
            set
            {
                _constituent.Material = _constituent.Material.AllConstituents[value];
                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(EmbodiedCarbon));
                _parent.UpdateAll();
            }
        }

        public string Notes => _constituent.Material.Notes;

        public double EmbodiedCarbon => _constituent.Material.EmbodiedCarbon;

        public double Proportion
        {
            get
            {
                return _constituent.Proportion;
            }
            set
            {
                _constituent.Proportion = value;
                _parent.UpdateAll();
            }
        }

        public ConstituentType ConType
        {
            get
            {
                if (_constituent.Material is CarbonMaterials.ICE3CementModel)
                {
                    return ConstituentType.CEMENT;
                }
                else
                    return ConstituentType.GENERAL;
            }
        }

        public CarbonMaterials.IConstituentMaterial Material
        {
            get
            {
                return _constituent.Material;
            }
        }

        public ConstituentVM(CarbonMaterials.CementAndConcreteConstituent constituent, IViewModelParent parent)
        {
            _constituent = constituent;
            _parent = parent;
        }


        ICommand _editCementCommand;

        public ICommand EditCementCommand
        {
            get
            {
                return _editCementCommand ?? (_editCementCommand = new CommandHandler(() => editCement(), true));
            }
        }

        void editCement()
        {
            CementVM vm = new CementVM(_constituent.Material as CarbonMaterials.ICE3CementModel);
            Window myWin = new Window
            {
                Content = new CementControl(),
                DataContext = vm,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            myWin.ShowDialog();        
        }


    }

    public enum ConstituentType
    {
        CEMENT,
        GENERAL
    }
}
