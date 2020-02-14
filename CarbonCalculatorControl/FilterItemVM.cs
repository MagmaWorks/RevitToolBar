using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonCalculator
{
    public class FilterItemVM :ViewModelBase
    {
        public string Name { get; set; }
        bool _isSelected;
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
        public FilterItemVM(string name, FilterSetVM parent)
        {
            Name = name;

            _parent = parent;
        }

        FilterSetVM _parent;
    }
}
