using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public FilterItemVM(string name)
        {
            Name = name;
        }
    }
}
