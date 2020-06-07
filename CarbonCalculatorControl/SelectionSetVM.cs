using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;

namespace CarbonCalculator
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SelectionSetVM : ViewModelBase
    {
        [JsonProperty]
        public List<FilterSelection> SelectedFilterValues { get; }
        [JsonProperty]
        public List<string> ElementsIdsToInclude { get; private set; }
        [JsonProperty]
        public List<string> ElementsIdsToExclude { get; private set; }
        [JsonProperty]
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

        public void setParent(ModelVM parent)
        {
            _parent = parent;
        }

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
                retStr = string.Format(@"Material {0}: {1}. ", SelectedMaterial, "" /*_parent.Materials[SelectedMaterial].Name*/);
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

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FilterSelection : ViewModelBase
    {
        [JsonProperty]
        public string FilterName { get; private set; }
        [JsonProperty]
        public string FilterValue { get; private set; }

        public FilterSelection(string name, string value)
        {
            FilterName = name;
            FilterValue = value;
        }
    }
}
