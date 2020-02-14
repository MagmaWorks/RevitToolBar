using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonCalculator
{
    public class SelectionSetVM : ViewModelBase
    {
        public List<FilterSelection> SelectedFilterValues { get; }
        public List<string> ElementsIdsToInclude { get; }
        public List<string> ElementsIdsToExclude { get; }
        public int SelectedMaterial { get; set; }
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
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        ModelVM _parent;

        public SelectionSetVM(int selectedMaterial, ModelVM parent)
        {
            _parent = parent;
            SelectedFilterValues = new List<FilterSelection>();
            ElementsIdsToInclude = new List<string>();
            ElementsIdsToExclude = new List<string>();
            SelectedMaterial = selectedMaterial;
        }

        public string Name
        {
            get
            {
                //return string.Format(@"Material {3}: {4}, {0} filters, {1} inclusions, {2} exclusions", SelectedFilterValues.Count, ElementsIdsToInclude.Count, ElementsIdsToExclude.Count, SelectedMaterial, _parent.Materials[SelectedMaterial].Name);
                string retStr = "";
                retStr = string.Format(@"Material {0}: {1}. ", SelectedMaterial, _parent.Materials[SelectedMaterial].Name);
                foreach (var item in SelectedFilterValues)
                {
                    retStr += item.FilterName + "=" + item.FilterValue + "; ";
                }
                retStr += ". ";
                if (ElementsIdsToInclude.Count > 0)
                {
                    retStr += "Include: ";
                    foreach (var elem in ElementsIdsToInclude)
                    {
                        retStr += elem + "; ";
                    }
                    retStr += ". ";
                }
                if (ElementsIdsToExclude.Count > 0)
                {
                    retStr += "Exclude: ";
                    foreach (var elem in ElementsIdsToExclude)
                    {
                        retStr += elem + "; ";
                    }
                    retStr += ". ";
                }
                return retStr;
            }
        }

        ICommand _moveUpCommand;

        public ICommand MoveUpCommand
        {
            get
            {
                return _moveUpCommand ?? (_moveUpCommand = new CommandHandler(() => moveUp(), true));
            }
        }

        void moveUp()
        {
            _parent.MoveUp(this);
        }

        ICommand _moveDownCommand;

        public ICommand MoveDownCommand
        {
            get
            {
                return _moveDownCommand ?? (_moveDownCommand = new CommandHandler(() => moveDown(), true));
            }
        }

        void moveDown()
        {
            _parent.MoveDown(this);
        }
    }

    public class FilterSelection : ViewModelBase
    {
        public string FilterName { get; }
        public string FilterValue { get; }
        public FilterSelection(string name, string value)
        {
            FilterName = name;
            FilterValue = value;
        }
    }
}
