using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public SelectionSetVM(int selectedMaterial)
        {
            SelectedFilterValues = new List<FilterSelection>();
            ElementsIdsToInclude = new List<string>();
            ElementsIdsToExclude = new List<string>();
            SelectedMaterial = selectedMaterial;
        }

        public string Name
        {
            get
            {
                return string.Format(@"{0} filters, {1} inclusions, {2} exclusions, material {3}", SelectedFilterValues.Count, ElementsIdsToInclude.Count, ElementsIdsToExclude.Count, SelectedMaterial);
            }
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
